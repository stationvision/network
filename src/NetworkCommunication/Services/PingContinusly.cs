//using Microsoft.Extensions.Options;
//using NetworkCommunications.Options;
//using NetworkCommunications.TcpManager;
//using System.Net;
//using System.Net.Sockets;

//public class ContinuousPingService : IHostedService, IDisposable
//{
//    private readonly ILogger<ContinuousPingService> _logger;
//    private readonly IOptions<ConfigOption> _options;
//    private Timer _timer;

//    public ContinuousPingService(ILogger<ContinuousPingService> logger, IOptions<ConfigOption> options)
//    {
//        _logger = logger;
//        _options = options;
//    }

//    public Task StartAsync(CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("Continuous Ping Service starting.");

//        _timer = new Timer(SendPing, null, TimeSpan.Zero, TimeSpan.FromMicroseconds(_options.Value.PingInMiliSecond));

//        return Task.CompletedTask;
//    }

//    private void SendPing(object state)
//    {
//        var clients = _options.Value.ClientIPs;

//        foreach (var C in clients)
//        {
//            try
//            {
//                var ip = IPAddress.Parse(C.Split(":")[0].ToString());
//                int port = int.Parse(C.Split(":")[1].ToString());


//                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
//                {
//                    socket.Connect(ip, port);
//                    var message = PingMessageGenerator.CreatePingMessage();
//                    //var message = PingMessageGenerator.CreatePingTextMessage();
//                    socket.Send(message);
//                    var timeoutMessage = PingMessageGenerator.CreatePingMessage();
//                    socket.Send(timeoutMessage);
//                }
//                _logger.LogInformation($"Ping sent to client {ip}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error sending ping to client");
//            }
//        }
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("Continuous Ping Service stopping.");

//        _timer?.Change(Timeout.Infinite, 0);

//        return Task.CompletedTask;
//    }

//    public void Dispose()
//    {
//        _timer?.Dispose();
//    }
//}
