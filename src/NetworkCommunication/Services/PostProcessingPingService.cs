//using Microsoft.Extensions.Options;
//using NetworkCommunications.Options;
//using NetworkCommunications.TcpManager;
//using System.Net;
//using System.Net.Sockets;

//public class PostProcessingPingService : IHostedService, IDisposable
//{
//    private readonly ILogger<PostProcessingPingService> _logger;
//    private readonly IOptions<ConfigOption> _options;
//    private readonly MessageProcessingService _messageProcessingService;

//    public PostProcessingPingService(ILogger<PostProcessingPingService> logger, IOptions<ConfigOption> options, MessageProcessingService messageProcessingService)
//    {
//        _logger = logger;
//        _options = options;
//        _messageProcessingService = messageProcessingService;
//        _messageProcessingService.MessageProcessed += OnMessageProcessed;
//    }

//    public Task StartAsync(CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("Post Processing Ping Service starting.");
//        return Task.CompletedTask;
//    }
//    private byte[] CreatePingMessage()
//    {
//        byte[] message = new byte[16];

//        // Fixed Header
//        message[0] = 0x66;
//        message[1] = 0x99;

//        // Current DateTime
//        DateTime now = DateTime.UtcNow;

//        // Hour
//        message[2] = (byte)now.Hour;
//        // Minute
//        message[3] = (byte)now.Minute;
//        // Second
//        message[4] = (byte)now.Second;
//        // Month
//        message[5] = (byte)now.Month;
//        // Date
//        message[6] = (byte)now.Day;
//        // Year (offset by 2000 for simplicity)
//        message[7] = (byte)(now.Year - 2000);

//        // Reserved (6 bytes set to 0x00)
//        for (int i = 8; i < 16; i++)
//        {
//            message[i] = 0x00;
//        }

//        return message;
//    }
//    private void OnMessageProcessed(object sender, MessageProcessedEventArgs e)
//    {
//        var clients = _options.Value.ClientIPs;

//        foreach (var client in clients)
//        {
//            try
//            {
//                var ip = IPAddress.Parse(client.Split(":")[0]);
//                int port = int.Parse(client.Split(":")[1].ToString());
//                var message = PingMessageGenerator.CreatePingMessage();
//                //var message = PingMessageGenerator.CreatePingTextMessage();
//                var timeoutMessage = PingMessageGenerator.CreatePingMessage();

//                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
//                {
//                    socket.Connect(ip, port);
//                    socket.Send(message);
//                    socket.Send(timeoutMessage);
//                }
//                _logger.LogInformation($"Post-processing ping sent to client {client}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error sending post-processing ping to client {client}");
//            }
//        }
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("Post Processing Ping Service stopping.");
//        return Task.CompletedTask;
//    }

//    public void Dispose()
//    {
//        _messageProcessingService.MessageProcessed -= OnMessageProcessed;
//    }
//}
