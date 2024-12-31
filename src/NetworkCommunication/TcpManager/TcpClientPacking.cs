using Microsoft.Extensions.Options;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;
using NetworkCommunications.Extentions;
using NetworkCommunications.Options;
using NetworkCommunications.Services;
using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace NetworkCommunications.TcpManager
{
    public class TcpServerPackingService : IHostedService, IDisposable
    {
        private readonly ILogger<TcpServerPackingService> _logger;
        private readonly IOptions<ConfigOption> _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly int _port;
        private readonly IPAddress _localAddr;
        private Socket _listener;
        private CancellationTokenSource _cts;
        private readonly ActionBlock<CommandMessage> _messageProcessingBlock;
        private readonly ConcurrentQueue<CommandMessage> _messageQueue;
        private readonly SemaphoreSlim _messageQueueSemaphore;
        private readonly uint[] _crcTable;
        private readonly byte[] _xorKey = new byte[2];
        private const ushort HEADER_VALUE = 0x7377;

        public TcpServerPackingService(ILogger<TcpServerPackingService> logger, IOptions<ConfigOption> options, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _options = options;
            _serviceProvider = serviceProvider;
            _localAddr = IPAddress.Parse(options.Value.ServerIP);
            _port = options.Value.ServerPort;
            _cts = new CancellationTokenSource();

            _messageQueue = new ConcurrentQueue<CommandMessage>();
            _messageQueueSemaphore = new SemaphoreSlim(0, int.MaxValue);

            _messageProcessingBlock = new ActionBlock<CommandMessage>(
                async message => await ProcessMessageAsync(message).ConfigureAwait(false),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1
                });

            _crcTable = new uint[256];
            InitializeCRCTable();
        }

        private void InitializeCRCTable()
        {
            for (uint i = 0; i < 256; i++)
            {
                uint crc = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ 0xEDB88320;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
                _crcTable[i] = crc;
            }
        }

        private uint CalculateCRC32(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            uint crc = 0xFFFFFFFF;
            
            for (int i = 0; i < data.Length; i++)
            {
                byte index = (byte)((crc & 0xFF) ^ data[i]);
                crc = (crc >> 8) ^ _crcTable[index];
            }
            
            return ~crc;
        }

        private string DecodeMessage(byte[] data)
        {
            try
            {
                if (data == null || data.Length < 6)
                {
                    _logger.LogWarning("Invalid data length");
                    return null;
                }

                string hexString = BitConverter.ToString(data).Replace("-", "");
                _logger.LogInformation($"Processing hex string: {hexString}");

                if (!hexString.StartsWith("7377"))
                {
                    _logger.LogWarning($"Invalid header: {hexString.Substring(0, 4)}");
                    return null;
                }

                string xorKeyHex = hexString.Substring(4, 4);
                _xorKey[0] = Convert.ToByte(xorKeyHex.Substring(0, 2), 16);
                _xorKey[1] = Convert.ToByte(xorKeyHex.Substring(2, 2), 16);
                _logger.LogInformation($"XOR Key: {_xorKey[0]:X2}{_xorKey[1]:X2}");

                string dataHex = hexString.Substring(8);
                string messageHex = dataHex.Substring(0, dataHex.Length - 8);
                string crcHex = dataHex.Substring(dataHex.Length - 8);

                byte[] decodedBytes = new byte[messageHex.Length / 2];
                for (int i = 0; i < messageHex.Length; i += 2)
                {
                    byte dataByte = Convert.ToByte(messageHex.Substring(i, 2), 16);
                    byte xorByte = _xorKey[(i/2) % 2];
                    decodedBytes[i/2] = (byte)(dataByte ^ xorByte);
                }

                uint receivedCrc = Convert.ToUInt32(crcHex, 16);
                uint calculatedCrc = CalculateCRC32(decodedBytes);

                _logger.LogInformation($"Received CRC: {receivedCrc:X8}");
                _logger.LogInformation($"Calculated CRC: {calculatedCrc:X8}");

                if (receivedCrc != calculatedCrc)
                {
                    _logger.LogWarning($"CRC mismatch. Received: {receivedCrc:X8}, Calculated: {calculatedCrc:X8}");
                    _logger.LogInformation($"Original message hex: {messageHex}");
                    _logger.LogInformation($"Decoded bytes: {BitConverter.ToString(decodedBytes)}");
                    return null;
                }

                string result = Encoding.ASCII.GetString(decodedBytes);
                _logger.LogInformation($"Decoded message: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decoding message");
                return null;
            }
        }

        private byte[] DecryptData(byte[] messageBytes)
        {
            // استخراج کلید XOR (بایت‌های 2 و 3)
            byte key1 = messageBytes[2];  // بایت اول کلید
            byte key2 = messageBytes[3];  // بایت دوم کلید
            ushort xor_key = (ushort)((key1 << 8) | key2);

            // جداسازی داده رمزشده (بدون هدر و CRC)
            int dataLength = messageBytes.Length - 8;
            byte[] encryptedData = new byte[dataLength];
            Array.Copy(messageBytes, 4, encryptedData, 0, dataLength);

            // رمزگشایی با استفاده از الگوریتم XOR
            byte[] decryptedData = new byte[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                // استفاده از بایت مناسب از کلید XOR (مشابه کد C)
                decryptedData[i] = (byte)(encryptedData[i] ^ 
                    ((i % 2 == 0) ? (xor_key & 0xFF) : ((xor_key >> 8) & 0xFF)));
            }

            return decryptedData;
        }

        private async Task HandleClientAsync(Socket handler, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    int bytesRead = await handler.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
                    if (bytesRead == 0) break;

                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    messageBuilder.Append(receivedData);
                    string completeMessage = messageBuilder.ToString();
                    
                    _logger.LogInformation($"پیام دریافت شده: {completeMessage}");

                    // تبدیل رشته هگز به بایت
                    byte[] messageBytes = new byte[completeMessage.Length / 2];
                    for (int i = 0; i < messageBytes.Length; i++)
                    {
                        messageBytes[i] = Convert.ToByte(completeMessage.Substring(i * 2, 2), 16);
                    }

                    // رمزگشایی داده با استفاده از الگوریتم جدید
                    byte[] decryptedData = DecryptData(messageBytes);
                    string decryptedHex = BitConverter.ToString(decryptedData).Replace("-", "");
                    _logger.LogInformation($"داده رمزگشایی شده (هگز): {decryptedHex}");

                    // استخراج و بررسی CRC
                    byte[] crcBytes = new byte[4];
                    Array.Copy(messageBytes, messageBytes.Length - 4, crcBytes, 0, 4);
                    uint receivedCrc = BitConverter.ToUInt32(crcBytes, 0);

                    // محاسبه CRC روی داده رمزگشایی شده
                    uint calculatedCrc = CalculateCRC32(decryptedData);

                    _logger.LogInformation($"داده رمزگشایی شده: {BitConverter.ToString(decryptedData)}");
                    _logger.LogInformation($"CRC دریافتی: 0x{receivedCrc:X8}");
                    _logger.LogInformation($"CRC محاسبه شده: 0x{calculatedCrc:X8}");

                    if (calculatedCrc != receivedCrc)
                    {
                        _logger.LogWarning($"CRC نامعتبر. دریافتی: 0x{receivedCrc:X8}, محاسبه شده: 0x{calculatedCrc:X8}");
                        messageBuilder.Clear();
                        continue;
                    }

                    string decodedMessage = Encoding.ASCII.GetString(decryptedData);
                    var message = new CommandMessage { Content = decodedMessage };
                    
                    _messageQueue.Enqueue(message);
                    _messageQueueSemaphore.Release();
                    
                    _logger.LogInformation($"پیام رمزگشایی شده: {decodedMessage}");
                    messageBuilder.Clear();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در پردازش داده دریافتی");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting TCP Server...");
            _ = Task.Run(() => StartTcpServerAsync(_cts.Token), cancellationToken);
            _ = Task.Run(() => ProcessMessagesFromQueue(_cts.Token), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping TCP Server...");
            _cts.Cancel();
            _listener?.Close();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _listener?.Dispose();
        }

        private async Task StartTcpServerAsync(CancellationToken cancellationToken)
        {
            IPEndPoint localEndPoint = new IPEndPoint(_localAddr, _port);
            _listener = new Socket(_localAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _listener.Bind(localEndPoint);
                _listener.Listen(100);

                _logger.LogInformation($"Server started on port {_port}.");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var handler = await _listener.AcceptAsync();
                    _logger.LogInformation("Connected to client.");
                    _ = HandleClientAsync(handler, cancellationToken);
                    _ = StartPingLoopAsync(handler, TimeSpan.FromMilliseconds(_options.Value.PingInMiliSecond), cancellationToken);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server encountered an error.");
            }
        }

        private async Task StartPingLoopAsync(Socket handler, TimeSpan interval, CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested && handler.Connected)
                {
                    await SendPingAsync(handler, cancellationToken);
                    await Task.Delay(interval, cancellationToken);
                }
            }, cancellationToken);
        }
        private async Task SendPingAsync(Socket handler, CancellationToken cancellationToken)
        {
            try
            {
                if (handler.Connected)
                {
                    var message = PingMessageGenerator.CreatePingTextMessage();
                    handler.SendTo(message, handler.RemoteEndPoint);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending ping to client.");
            }
        }


        private async Task ProcessMessagesFromQueue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _messageQueueSemaphore.WaitAsync(cancellationToken);

                while (_messageQueue.TryDequeue(out var message))
                {
                    await _messageProcessingBlock.SendAsync(message);
                }
            }
        }


        private bool ValidateMessage(CommandMessage messagecommand)
        {
            string pattern = @"^[EWZ],[A-Za-z0-9]{1,10},\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4}, ?\d{1,4},\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3},\d{1,3}:\d{1,3}:\d{1,3}:\d{1,3},\d{1,3},\d{1,2}/\d{1,2}/\d{1,2},\d{1,2}:\d{1,2}:\d{1,2}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(messagecommand.Content);
        }
        private bool NeedsCleanup(CommandMessage message)
        {
            return message.Content.Contains(" ,");
        }

        private string CleanUpMessage(CommandMessage message)
        {
            // Split the message by commas and remove spaces from each part
            var parts = message.Content.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }
            // Rejoin the parts into a single string
            return string.Join(",", parts);
        }

        private async Task ProcessMessageAsync(CommandMessage message)
        {
            try
            {
                if (!message.Content.StartsWith("E"))
                {
                    var ss = message.Content;
                }

                if (NeedsCleanup(message))
                {
                    message.Content = CleanUpMessage(message);
                }
                var validmessage = ValidateMessage(message);



                if (!validmessage)
                {
                    return;
                }
                Console.WriteLine($"Received message: {message.Content.Split(",")[22] + "" + message.Content.Split(",")[23]}");

                // Create a new scope to resolve scoped services
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IRepository<MessageEntity>>();
                    var messageProcessingService = scope.ServiceProvider.GetRequiredService<MessageProcessingService>();

                    // Process the message (e.g., save to the database)
                    var ArrayContent = message.Content.Split(",");
                    var messageType = ArrayContent.DetermineMessageType();
                    string pulsDate = ArrayContent.GetPulsDate();

                    DateTime parsedDate = DateTime.ParseExact(pulsDate, "MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture);

                    var messageEntity = new MessageEntity
                    {
                        Content = message.Content,
                        ErrorDetails = "",
                        MessageId = Guid.NewGuid().ToString(),
                        Timestamp = DateTime.Now,
                        Status = Enums.MessageStatus.Pending,
                        MessageType = messageType.ToString(),
                        BoardId = ArrayContent.GetBoardId(),
                        PulsData = parsedDate
                    };  

                    repository.Add(messageEntity);
                    repository.SaveChanges();

                    // Add message to buffer
                    var type = new MessageStoreType() { StartDate = messageEntity.PulsData, MessageId = Guid.Parse(messageEntity.MessageId) };
                    messageProcessingService.AddMessage(messageEntity, type);
                    await messageProcessingService.StartAsync(CancellationToken.None);
                    Console.WriteLine($"Pulse Date : {messageEntity.PulsData}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message.");
            }
        }
    }
}
