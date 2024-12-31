using Microsoft.Extensions.Options;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;
using NetworkCommunications.Dtos;
using NetworkCommunications.Extentions;
using NetworkCommunications.MethodTypes;
using NetworkCommunications.Options;
using System.Collections.Concurrent;


public class MessageProcessedEventArgs : EventArgs
{
    public MessageEntity Message { get; set; }
}
public class MessageProcessingService : IHostedService, IDisposable
{
    private readonly ILogger<MessageProcessingService> _logger;
    private readonly ConcurrentDictionary<MessageStoreType, MessageBuffer> _messageBuffers;
    private readonly ConcurrentDictionary<Guid, object> _boardLocks;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<ConfigOption> _options;
    private CancellationTokenSource _cts;
    private Task _backgroundTask;
    private readonly SemaphoreSlim _processingSemaphore = new SemaphoreSlim(1, 1);

    private const ushort HEADER_VALUE = 0x7377;
    private byte[] _xorKey = new byte[2];

    public MessageProcessingService(
        ILogger<MessageProcessingService> logger,
        IServiceProvider serviceProvider, 
        IOptions<ConfigOption> options)
    {
        _logger = logger;
        _messageBuffers = new ConcurrentDictionary<MessageStoreType, MessageBuffer>();
        _boardLocks = new ConcurrentDictionary<Guid, object>();
        _serviceProvider = serviceProvider;
        _options = options;
    }

    public void AddMessage(MessageEntity messageEntity, MessageStoreType storeType)
    {
        var type = new MessageStoreType() { MessageId = storeType.MessageId, StartDate = storeType.StartDate };
        var buffer = _messageBuffers.GetOrAdd(type, new MessageBuffer(type));
        buffer.AddMessage(messageEntity);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
     _backgroundTask = Task.Run(() => ProcessMessagesAsync(_cts.Token), _cts.Token);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_backgroundTask == null) return;

        try
        {
            _cts.Cancel();
        }
        finally
        {
            await Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
    MessageEntity previousMessage = null;
    ClientPuls previousPuls = null;
    private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
    {

        while (!cancellationToken.IsCancellationRequested)
        {
            var msgbuffer = _messageBuffers.OrderBy(x => x.Value.StartDate).ToDictionary(x => x.Key, x => x.Value);

            foreach (var message in msgbuffer.Keys)
            {
                var lockObj = _boardLocks.GetOrAdd(message.MessageId, new object());
                await _processingSemaphore.WaitAsync(cancellationToken);

                try
                {
                    lock (lockObj)
                    {
                        if (_messageBuffers.TryGetValue(message, out var buffer) && !buffer.IsEmpty)
                        {
                            while (buffer.TryGetNextMessage(out var currentMessage))
                            {
                                Console.WriteLine(currentMessage.PulsData);
                                AggregateMessagesAsync(currentMessage, previousMessage, cancellationToken);
                                previousMessage = currentMessage; // Update previousMessage to currentMessage

                                Task.Delay(100);
                                MessageProcessed?.Invoke(this, new MessageProcessedEventArgs { Message = currentMessage });

                            }
                        }
                    }
                }
                finally
                {
                    _processingSemaphore.Release();
                }
            }
            await Task.Delay(100, cancellationToken); // Adjust the delay as needed
        }
    }

    ClientPuls GetLatesPuls(MessageEntity message, ClientPuls clientPuls, IRepository<ClientPuls> repository)
    {
        if (message is null) return null;
        var latestPuls = repository.Query(x => x.MessageId == message.MessageId && x.PulsId == clientPuls.PulsId)
            .OrderByDescending(x => x.Timestamp).FirstOrDefault();

        return latestPuls;
    }

    int count = 0;
    public event EventHandler<MessageProcessedEventArgs> MessageProcessed;

    private async Task AggregateMessagesAsync(MessageEntity message, MessageEntity PrevousePuls, CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var messageEntityRepository = scope.ServiceProvider.GetRequiredService<IRepository<MessageEntity>>();
            var pulsRepository = scope.ServiceProvider.GetRequiredService<IRepository<Puls>>();
            var clientPulsRepository = scope.ServiceProvider.GetRequiredService<IRepository<ClientPuls>>();

            try
            {
                count++;
                var messagePulses = BindClientPuls(message, pulsRepository, clientPulsRepository);
                messagePulses = messagePulses.OrderBy(x => x.puls.Name.Length).ThenBy(x => x.puls.Name).ToList();
                ClientPuls latestPuls = null;
                foreach (var newPuls in messagePulses)
                {
                    var newPulsRange = new ClientToServerDto().GetRangeName(newPuls);

                    latestPuls = GetLatesPuls(PrevousePuls, newPuls, clientPulsRepository);

                    if (latestPuls == null)
                    {
                        CreateNewClientPulsRecord(newPuls, latestPuls, clientPulsRepository, newPulsRange);
                        continue;
                    }

                    var latestPulsRange = new ClientToServerDto().GetRangeName(latestPuls);
                    if (!IsNewMessageDifferentFromLatestMessage(latestPuls, newPuls))
                    {
                        EmptyDeviationTime(latestPuls);
                        ChangeIncreaseProductionTime(false, latestPuls);
                        UpdateLatestClientPulsStartDate(latestPuls, newPuls, clientPulsRepository, newPulsRange);
                        continue;
                    }

                    if (latestPulsRange.RangeName == newPulsRange.RangeName &&
                        Math.Abs(latestPuls.Value.ToDecimal().Value - newPuls.Value.ToDecimal().Value)
                        <= latestPulsRange.IgnoredDifferenceThreshold)
                    {
                        EmptyDeviationTime(latestPuls);
                        ChangeIncreaseProductionTime(false, latestPuls);
                        UpdateLatestClientPulsStartDate(latestPuls, newPuls, clientPulsRepository, newPulsRange);
                        continue;
                    }
                    else
                    {
                        if (!latestPuls.DeviationTime.HasValue)
                        {
                            SetPulsDateToDeviationTime(newPuls, latestPuls);
                        }

                        IFDeviationDifferenceLessThanIgnoredDuration(newPuls, latestPuls, clientPulsRepository, newPulsRange, latestPulsRange);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine($"Message processed: {message.Content}");
            }
        }
    }

    private List<ClientPuls> BindClientPuls(MessageEntity NewMessage, IRepository<Puls> PulsRepository, IRepository<ClientPuls> ClientPulsRepository)
    {

        var result = new List<ClientPuls>();
        if (NewMessage is null)
        {
            return result;
        }
        string[] splitArray = NewMessage.Content.Split(',');
        List<string> incommingData = new List<string>(splitArray);


        var Pulsdatas = PulsRepository.Query(x => x.BoardId == NewMessage.BoardId).OrderBy(x => x.Name).OrderBy(x => x.Name.Length).ToList();
        string PulsNamePrefix = "ADC";

        foreach (var item in Pulsdatas)
        {

            if (item.Name == PulsNamePrefix + 0)
            {
                var res = CreateData(NewMessage, item, incommingData[2], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 1)
            {
                var res = CreateData(NewMessage, item, incommingData[3], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 2)
            {
                var res = CreateData(NewMessage, item, incommingData[4], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 3)
            {
                var res = CreateData(NewMessage, item, incommingData[5], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 4)
            {
                var res = CreateData(NewMessage, item, incommingData[6], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 5)
            {
                var res = CreateData(NewMessage, item, incommingData[7], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 6)
            {
                var res = CreateData(NewMessage, item, incommingData[8], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 7)
            {
                var res = CreateData(NewMessage, item, incommingData[9], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 8)
            {
                var res = CreateData(NewMessage, item, incommingData[10], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 9)
            {
                var res = CreateData(NewMessage, item, incommingData[11], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 10)
            {
                var res = CreateData(NewMessage, item, incommingData[12], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 11)
            {
                var res = CreateData(NewMessage, item, incommingData[13], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 12)
            {
                var res = CreateData(NewMessage, item, incommingData[14], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 13)
            {
                var res = CreateData(NewMessage, item, incommingData[15], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 14)
            {
                var res = CreateData(NewMessage, item, incommingData[16], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 15)
            {
                var res = CreateData(NewMessage, item, incommingData[17], incommingData);
                result.Add(res);
                continue;
            }
            if (item.Name == PulsNamePrefix + 16)
            {
                var res = CreateData(NewMessage, item, incommingData[18], incommingData);
                result.Add(res);
                continue;
            }
        }
        return result;
    }

    private ClientPuls CreateData(MessageEntity newMessage, Puls item, string value, List<string> incomingData)
    {
        return new ClientPuls
        {
            puls = item,
            PulsId = item.Id,
            Value = value,
            Count = 0,
            BoardId = newMessage.BoardId,
            Timestamp = DateTime.Now,
            StartDate = DateTime.Parse(incomingData[22] + " " + incomingData[23]),
            Hash = value.ComputeSha256Hash(),
            MessageId = newMessage.MessageId,

        };
    }

    private void CleanBuffer(Guid messageId)
    {
        //_messageBuffers.TryRemove(messageId, out _);
    }

    private void SetPulsDateToDeviationTime(ClientPuls newPuls, ClientPuls latestPuls)
    {
        //var timeSpan = DateTime.Now - newPuls.StartDate.Value;
        var timeSpan = newPuls.StartDate.Value - latestPuls.StartDate.Value;
        latestPuls.DeviationTime = timeSpan.TotalSeconds;
    }

    private void IFDeviationDifferenceLessThanIgnoredDuration(ClientPuls newPuls, ClientPuls latestPuls,
        IRepository<ClientPuls> clientPulsRepository, ValidationResultItem newPulsRange, ValidationResultItem latestPulsRange)
    {
        if (latestPuls.DeviationTime < latestPulsRange.IgnoredDurationThreshold)
        {
            UpdateLatestClientPulsStartDate(latestPuls, newPuls, clientPulsRepository, newPulsRange);

            if (!latestPuls.IncreaseProductionTime)
            {
                ChangeIncreaseProductionTime(true, latestPuls);
                UpdateLatestClientPulsEndDateAndIncreaseCount(latestPuls, newPuls, clientPulsRepository, newPulsRange);
            }
        }
        else
        {
            CommitLatestClientPuls(latestPuls, newPuls, clientPulsRepository);
            CreateNewClientPulsRecord(newPuls, latestPuls, clientPulsRepository, newPulsRange);
        }
    }

    private void EmptyDeviationTime(ClientPuls puls)
    {
        puls.DeviationTime = null;
    }

    private void ChangeIncreaseProductionTime(bool status, ClientPuls puls)
    {
        puls.IncreaseProductionTime = status;
    }

    private void CreateNewClientPulsRecord(ClientPuls newPuls, ClientPuls latespuls, IRepository<ClientPuls> clientPulsRepository, ValidationResultItem validationResultItem)
    {

        if (latespuls != null)
        {
            newPuls.MessageId = latespuls.MessageId;
        }
        newPuls.Timestamp = DateTime.Now;
        newPuls.status = validationResultItem.RangeName;
        clientPulsRepository.Add(newPuls);
        clientPulsRepository.SaveChanges();
    }

    private void CommitLatestClientPuls(ClientPuls latestPuls, ClientPuls newPuls, IRepository<ClientPuls> repository)
    {
        if (latestPuls == null) return;

        latestPuls.EndDate = newPuls.StartDate;
        latestPuls.MessageId = newPuls.MessageId;
        latestPuls.Timestamp = DateTime.Now;
        repository.Update(latestPuls);
        repository.SaveChanges();
    }

    private void UpdateLatestClientPulsEndDateAndIncreaseCount(ClientPuls latestPuls, ClientPuls newPuls, IRepository<ClientPuls> repository, ValidationResultItem newPulsRange)
    {
        latestPuls.EndDate = newPuls.StartDate;
        latestPuls.MessageId = newPuls.MessageId;
        latestPuls.Timestamp = DateTime.Now;
        latestPuls.status = newPulsRange.RangeName;
        latestPuls.Count++;
        repository.Update(latestPuls);
        repository.SaveChanges();
    }

    private bool IsNewMessageDifferentFromLatestMessage(ClientPuls? latestPuls, ClientPuls newPuls)
    {
        return latestPuls == null || latestPuls.Hash != newPuls.Hash;
    }

    private void UpdateLatestClientPulsStartDate(ClientPuls latestPuls, ClientPuls newPuls, IRepository<ClientPuls> repository, ValidationResultItem newPulsRange)
    {

        latestPuls.EndDate = newPuls.StartDate;
        latestPuls.MessageId = newPuls.MessageId;
        latestPuls.Timestamp = DateTime.Now;
        latestPuls.status = newPulsRange.RangeName;
        repository.Update(latestPuls);
        repository.SaveChanges();
    }

    public void Dispose()
    {
        _cts?.Cancel();
    }
}

public class MessageStoreType
{
    public DateTime StartDate { get; set; }
    public Guid MessageId { get; set; }
}
public class MessageBuffer
{
    private readonly List<MessageEntity> _messages;
    private static readonly object _messagesLock = new object();

    //public MessageBuffer(Guid messageId)
    public MessageBuffer(MessageStoreType messageStoreType)
    {
        MessageId = messageStoreType.MessageId;
        StartDate = messageStoreType.StartDate;
        _messages = new List<MessageEntity>();
    }

    public Guid MessageId { get; }
    public DateTime StartDate { get; }

    public void AddMessage(MessageEntity message)
    {
        lock (_messagesLock)
        {
            _messages.Add(message);
            _messages.Sort((x, y) => DateTime.Compare(x.PulsData, y.PulsData));
        }
    }

    public bool TryGetNextMessage(out MessageEntity message)
    {
        lock (_messagesLock)
        {
            if (_messages.Count > 0)
            {
                message = _messages[0];
                _messages.RemoveAt(0);
                return true;
            }
            message = null;
            return false;
        }
    }

    public bool IsEmpty
    {
        get
        {
            lock (_messagesLock)
            {
                return _messages.Count == 0;
            }
        }
    }
}
