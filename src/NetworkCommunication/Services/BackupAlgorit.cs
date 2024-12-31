//using Microsoft.Extensions.Options;
//using Monitoring.Db.Interfaces;
//using Monitoring.Db.Models;
//using NetworkCommunications.Dtos;
//using NetworkCommunications.Extentions;
//using NetworkCommunications.MethodTypes;
//using NetworkCommunications.Options;
//using System.Collections.Concurrent;
//using System.Text.Json;

//public class MessageProcessingService : IHostedService, IDisposable
//{
//    private readonly IServiceProvider _serviceProvider;
//    private readonly IOptions<ConfigOption> _options;
//    //private readonly ConcurrentDictionary<string, MessageBuffer> _messageBuffers = new ConcurrentDictionary<string, MessageBuffer>();
//    private Timer _aggregationTimer;
//    private readonly TimeSpan _aggregationInterval;
//    private readonly ConcurrentDictionary<string, Timer> _boardTimers = new ConcurrentDictionary<string, Timer>();
//    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // Semaphore for locking

//    public MessageProcessingService(IServiceProvider serviceProvider, IOptions<NetworkCommunications.Options.ConfigOption> options)
//    {
//        _serviceProvider = serviceProvider;
//        _options = options;
//        //_aggregationInterval = TimeSpan.FromSeconds(int.Parse(_options.Value.AcceptableDelayInSecond));



//        _messageBuffers = new ConcurrentDictionary<string, MessageBuffer>();
//        _boardSemaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

//    }

//    //public Task StartAsync(CancellationToken cancellationToken)
//    //{
//    //    //_aggregationTimer = new Timer(AggregateMessages, null, TimeSpan.Zero, _aggregationInterval);

//    //    return Task.CompletedTask;
//    //}

//    //public Task StopAsync(CancellationToken cancellationToken)
//    //{
//    //    //foreach (var timer in _boardTimers.Values)
//    //    //{
//    //    //    timer?.Change(Timeout.Infinite, 0);
//    //    //}
//    //    //_aggregationTimer?.Change(Timeout.Infinite, 0);
//    //    return Task.CompletedTask;
//    //}

//    //public void AddMessage(MessageEntity messageEntity, string boardId)
//    //{
//    //    var buffer = _messageBuffers.GetOrAdd(boardId, new MessageBuffer(boardId, _options));
//    //    buffer.AddMessage(messageEntity);

//    //    // Create or reset the timer for this boardId
//    //    _boardTimers.AddOrUpdate(boardId, key =>
//    //    {
//    //        var timer = new Timer(AggregateMessages, boardId, _aggregationInterval, Timeout.InfiniteTimeSpan);
//    //        return timer;
//    //    },
//    //    (key, existingTimer) =>
//    //    {
//    //        existingTimer.Change(_aggregationInterval, Timeout.InfiniteTimeSpan);
//    //        return existingTimer;
//    //    });
//    //}
//    private readonly ConcurrentDictionary<string, MessageBuffer> _messageBuffers;
//    private readonly ConcurrentDictionary<string, SemaphoreSlim> _boardSemaphores;
//    private CancellationTokenSource _cts;
//    private Task _backgroundTask;


//    public void AddMessage(MessageEntity messageEntity, string boardId)
//    {
//        var buffer = _messageBuffers.GetOrAdd(boardId, new MessageBuffer(boardId));
//        buffer.AddMessage(messageEntity);

//        var semaphore = _boardSemaphores.GetOrAdd(boardId, new SemaphoreSlim(1, 1));
//        semaphore.Release(); // Notify the background task about the new message
//    }

//    public Task StartAsync(CancellationToken cancellationToken)
//    {
//        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
//        _backgroundTask = Task.Run(() => ProcessMessagesAsync(_cts.Token), _cts.Token);

//        return Task.CompletedTask;
//    }

//    public async Task StopAsync(CancellationToken cancellationToken)
//    {
//        if (_backgroundTask == null) return;

//        try
//        {
//            _cts.Cancel();
//        }
//        finally
//        {
//            await Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
//        }
//    }
//    private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
//    {
//        while (!cancellationToken.IsCancellationRequested)
//        {
//            foreach (var boardId in _messageBuffers.Keys)
//            {
//                var semaphore = _boardSemaphores.GetOrAdd(boardId, new SemaphoreSlim(1, 1));
//                await semaphore.WaitAsync(cancellationToken);

//                if (_messageBuffers.TryGetValue(boardId, out var buffer))
//                {
//                    await AggregateMessages(buffer, cancellationToken);
//                }
//            }

//            await Task.Delay(100, cancellationToken); // Adjust the delay as needed
//        }
//    }



//    private double? _deviationTime = 10;
//    private bool _increaseProductionTime = false;
//    public async void AggregateMessages(object state)
//    {
//        int TotalMessageReceived = 0;
//        await _semaphore.WaitAsync(); // Wait to enter the critical section
//        try
//        {
//            using (var scope = _serviceProvider.CreateScope())
//            {
//                var aggregatedMessageRepository = scope.ServiceProvider.GetRequiredService<IRepository<ClientData>>();
//                var messageEntityRepository = scope.ServiceProvider.GetRequiredService<IRepository<MessageEntity>>();

//                foreach (var buffer in _messageBuffers.Values)
//                {
//                    //if (testing || buffer.ShouldAggregate())
//                    {
//                        Console.WriteLine($"Aggregating messages for board {buffer.BoardId}");

//                        var Messages = _messageBuffers.Where(x => x.Key == buffer.BoardId).ToList();
//                        var filteredBuffers = Messages.Where(x => x.Key == buffer.BoardId).SelectMany(x => x.Value.GetTimestamps());

//                        if (!filteredBuffers.Any())
//                        {
//                            continue;
//                        }

//                        //Step 1
//                        var latestMesssage = aggregatedMessageRepository.Query(x => x.DeviceId == buffer.BoardId).OrderByDescending(x => x.Timestamp).FirstOrDefault();


//                        var buffermessages = buffer.GetMessage().Content;
//                        var newgetdata = JsonSerializer.Deserialize<ClientToServerDto>(buffermessages.ToJson());

//                        var newMessage = new ClientData
//                        {
//                            DeviceId = buffer.BoardId,
//                            Timestamp = DateTime.Now,
//                            StartDate = DateTime.Now,
//                            Data = buffer.GetMessage().Content.ToJson(),
//                            Hash = buffer.GetMessage().Content.ComputeSha256Hash(),
//                            MachineName = _options.Value.MachineName,
//                            ADC2 = newgetdata.ADC2,
//                        };
//                        TotalMessageReceived++;
//                        if (latestMesssage is null)
//                        {
//                            var deSeriilizeLatestMessage = JsonSerializer.Deserialize<ClientToServerDto>(newMessage.Data);
//                            var getrange = await deSeriilizeLatestMessage.GetRangeName(newMessage);
//                            newMessage.Status = getrange.Values.FirstOrDefault();
//                            //means this is first message thats coming from the board
//                            CreateNewPulsRecord(newMessage, aggregatedMessageRepository);
//                            return;
//                        }


//                        bool NewMessageIsDifferent = IsNewMessageDifferentfromLatestMessage(latestMesssage, newMessage);

//                        if (Int64.Parse(newMessage.ADC2) >= 4095)
//                        {
//                            string a = newMessage.ADC2;
//                        }
//                        if (!NewMessageIsDifferent)
//                        {
//                            //Update OldMessage Start Date
//                            UpdateLatestMessageStartDate(latestMesssage, aggregatedMessageRepository, buffer);
//                            EmptyDeviationTime();
//                            ChangeIncreaseProductionTime(false);
//                            return;
//                        }


//                        // So , we have a new message that is different from the latest message
//                        // Now we need to check if the new message is in the desired puls range

//                        var changedPulses = GetNewMessageChangedPuls(newMessage, latestMesssage);

//                        var deSeriilizeOldMessage = JsonSerializer.Deserialize<ClientToServerDto>(latestMesssage.Data);
//                        var latestMessageValidation = deSeriilizeOldMessage.Validate(deSeriilizeOldMessage);

//                        foreach (var changed in changedPulses)
//                        {
//                            newMessage.Status = changed.Value.RangeName;
//                            var correspondingPrevousePuls = latestMessageValidation[changed.Key];

//                            //Does changed puls in same range as previous puls?
//                            if (correspondingPrevousePuls.RangeIndex == changed.Value.RangeIndex)
//                            {
//                                //Yes, then check for
//                                //Is the difference between the current number and the previous number less than the numerical difference that can be ignored?
//                                if (Math.Abs(correspondingPrevousePuls.Value.Value - changed.Value.Value.Value) <= correspondingPrevousePuls.IgnoredDifferenceThreshold)
//                                {
//                                    // If yes, then update OldMessage Start Date
//                                    UpdateLatestMessageStartDate(latestMesssage, aggregatedMessageRepository, buffer);
//                                    EmptyDeviationTime();
//                                    ChangeIncreaseProductionTime(false);
//                                    return;
//                                }
//                                else
//                                {
//                                    if (!_deviationTime.HasValue) //Thats new
//                                    {
//                                        SetPulsDateToDeviationTime(newMessage);
//                                    }
//                                    IFDeviationDiffrenceLessThanIgnorableduration(newMessage, latestMesssage, aggregatedMessageRepository, buffer, changed, correspondingPrevousePuls);
//                                    return;
//                                }
//                            }
//                            else
//                            {
//                                if (!_deviationTime.HasValue) //Thats new
//                                {
//                                    SetPulsDateToDeviationTime(newMessage);
//                                }
//                                IFDeviationDiffrenceLessThanIgnorableduration(newMessage, latestMesssage, aggregatedMessageRepository, buffer, changed, correspondingPrevousePuls);
//                                return;
//                            }
//                        }
//                        CleanBuffer(buffer.BoardId);
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//        }
//        finally
//        {
//            _semaphore.Release(); // Release the lock
//            Console.WriteLine($"Total Message Received: {TotalMessageReceived}");
//        }
//    }

//    private void CleanBuffer(string boardId)
//    {
//        //_messageBuffers.TryRemove(boardId, out _);
//    }

//    public bool testing = false;
//    private void SetPulsDateToDeviationTime(ClientData newMessage)
//    {
//        var dateTime = newMessage.StartDate; // Replace with your actual DateTime object
//        DateTime now = DateTime.Now;
//        TimeSpan timeSpan = now - dateTime;
//        double totalSeconds = timeSpan.TotalSeconds;
//        _deviationTime = totalSeconds;
//    }


//    private void IFDeviationDiffrenceLessThanIgnorableduration(ClientData newMessage, ClientData latestMesssage, IRepository<ClientData> aggregatedMessageRepository,
//        MessageBuffer buffer, KeyValuePair<string, ValidationResultItem> changed, ValidationResultItem CorrespondingMessage)
//    {
//        var dateTime = newMessage.StartDate; // Replace with your actual DateTime object
//        DateTime now = DateTime.Now;
//        TimeSpan timeSpan = now - dateTime;
//        double totalSeconds = timeSpan.TotalSeconds;


//        if (_deviationTime < CorrespondingMessage.IgnoredDurationThreshold)
//        {
//            UpdateLatestMessageStartDate(latestMesssage, aggregatedMessageRepository, buffer);
//            if (!_increaseProductionTime)
//            {
//                ChangeIncreaseProductionTime(true);
//                UpdateLatestMessageStartDateAndIncreaseCount(latestMesssage, aggregatedMessageRepository, buffer);
//                return;
//            }
//            return;

//        }
//        else
//        {
//            CommitLatestMessage(latestMesssage, aggregatedMessageRepository);
//            CreateNewPulsRecord(newMessage, aggregatedMessageRepository);
//            //TaieemNaze record bar asas Meghdar Daryafti ??
//            return;

//        }
//    }
//    private void EmptyDeviationTime()
//    {
//        _deviationTime = null;
//    }
//    private void ChangeIncreaseProductionTime(bool status)
//    {
//        _increaseProductionTime = status;
//    }
//    private void CreateNewPulsRecord(ClientData newMessage, IRepository<ClientData> aggregatedMessageRepository)
//    {

//        aggregatedMessageRepository.Add(newMessage);
//        aggregatedMessageRepository.SaveChanges();

//    }
//    private void CommitLatestMessage(ClientData latestMesssage, IRepository<ClientData> aggregatedMessageRepository)
//    {
//        if (latestMesssage == null)
//            return;
//        latestMesssage.EndDate = DateTime.Now;
//        aggregatedMessageRepository.Update(latestMesssage);
//        aggregatedMessageRepository.SaveChanges();
//    }
//    private void UpdateLatestMessageStartDateAndIncreaseCount(ClientData latestMesssage, IRepository<ClientData> repository, MessageBuffer buffer)
//    {
//        latestMesssage.EndDate = DateTime.Now;
//        int c = latestMesssage.Count;
//        c++;
//        latestMesssage.Count = c;
//        repository.Update(latestMesssage);
//        repository.SaveChanges();
//    }
//    private Dictionary<string, ValidationResultItem> GetNewMessageChangedPuls(ClientData newMessage, ClientData latestMessage)
//    {
//        var deSeriilizeNewMessage = JsonSerializer.Deserialize<ClientToServerDto>(newMessage.Data);
//        var deSeriilizeLatestMessage = JsonSerializer.Deserialize<ClientToServerDto>(latestMessage.Data);

//        var Validation = deSeriilizeNewMessage.Validate(deSeriilizeLatestMessage);
//        var changedPulses = Validation.Where(x => x.Value.PulsHasChanged)
//            .ToDictionary(x => x.Key, x => x.Value);
//        return changedPulses;
//    }
//    private bool IsNewMessageDifferentfromLatestMessage(ClientData? latestMesssage, ClientData newMessage)
//    {
//        if (latestMesssage is null) //Step2: Is new message different from the old message?
//            return true;

//        if (latestMesssage.Hash != newMessage.Hash)
//            return true;

//        return false;

//    }
//    private void UpdateLatestMessageStartDate(ClientData oldMessage, IRepository<ClientData> repository, MessageBuffer buffer)
//    {
//        oldMessage.EndDate = DateTime.Now;
//        repository.Update(oldMessage);
//        repository.SaveChanges();
//    }
//    private void ProcessChangedPulse(ClientData latestMessage, ClientData newMessage, IRepository<ClientData> repository, MessageBuffer buffer, (bool InRange, long? Difference, string Message, int RangeIndex, int IgnoredDifferenceThreshold, double IgnoredDurationThreshold, bool PulsHasChnaged) changed)
//    {
//        double duration = (DateTime.Now - latestMessage.EndDate.Value).TotalSeconds;

//        if (duration < changed.IgnoredDurationThreshold)
//        {
//            UpdateLatestMessageStartDateAndIncreaseCount(latestMessage, repository, buffer);
//        }
//        else
//        {
//            //Remember to check if there is AlreadyMessage in db(LatestMessage as we say) update EndDate to Now First then Insert New Puls Record
//            CommitLatestMessage(latestMessage, repository);
//            CreateNewPulsRecord(newMessage, repository);
//        }
//    }
//    public void Dispose()
//    {
//        foreach (var timer in _boardTimers.Values)
//        {
//            timer?.Dispose();
//        }
//        //_aggregationTimer?.Dispose();
//        _semaphore?.Dispose();
//    }
//}

//public class MessageBuffer
//{
//    private readonly object _lock = new object();
//    private readonly DateTime _startTime;
//    private readonly ConcurrentBag<MessageEntity> _messages;
//    private readonly ConfigOption _options;

//    public string BoardId { get; }

//    public MessageBuffer(string boardId, IOptions<ConfigOption> options)
//    {
//        BoardId = boardId;
//        _options = options.Value;
//        _startTime = DateTime.Now;
//        _messages = new ConcurrentBag<MessageEntity>();
//    }
//    public IEnumerable<DateTime> GetTimestamps()
//    {
//        return _messages.Select(m => m.Timestamp);
//    }

//    public void AddMessage(MessageEntity message)
//    {
//        lock (_lock)
//        {
//            _messages.Add(message);
//        }
//    }
//    public List<MessageEntity> GetMessages()
//    {
//        return _messages.ToList();
//    }
//    public MessageEntity GetMessage()
//    {
//        return _messages.FirstOrDefault();
//    }

//    public bool ShouldAggregate()
//    {
//        lock (_lock)
//        {
//            return (DateTime.Now - _startTime).TotalSeconds >= int.Parse(_options.AcceptableDelayInSecond);
//        }
//    }
//}
