//using Microsoft.Extensions.Options;
//using NetworkCommunications.Handlers;
//using NetworkCommunications.Options;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//namespace NetworkCommunications.Services
//{
//    public class TcpListenerService : BackgroundService
//    {
//        private readonly IEndpointInstance _endpointInstance;
//        private readonly ConfigOption _options;
//        private readonly MessageHandler _messageHandler;
//        private TcpListener _listener;

//        public TcpListenerService(IEndpointInstance endpointInstance, IOptions<ConfigOption> options)
//        {
//            _endpointInstance = endpointInstance;
//            _options = options.Value;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            var ip = IPAddress.Parse(_options.ServerIP);
//            _listener = new TcpListener(ip, int.Parse(_options.ServerPort.ToString()));
//            _listener.Start();
//            Console.WriteLine($"TCP Server started, listening on port {_options.ServerPort} ...");

//            try
//            {
//                while (!stoppingToken.IsCancellationRequested)
//                {
//                    if (_listener.Pending())
//                    {
//                        var client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
//                        _ = HandleClientAsync(client, stoppingToken);
//                    }
//                    else
//                    {
//                        await Task.Delay(100, stoppingToken).ConfigureAwait(false); // Slight delay to prevent tight loop
//                    }
//                }
//            }
//            catch (OperationCanceledException)
//            {
//                // Expected when stoppingToken is canceled
//            }
//            finally
//            {
//                _listener.Stop();
//                Console.WriteLine("TCP Server stopped.");
//            }
//        }

//        private async Task HandleClientAsync(TcpClient client, CancellationToken stoppingToken)
//        {
//            List<string> lines = new List<string>();

//            const int messageLength = 151; // Fixed length of each message
//            var buffer = new byte[messageLength * 10]; // Read up to 10 messages at once
//            var stream = client.GetStream();
//            var ipEndPoint = client.Client.RemoteEndPoint as IPEndPoint;

//            Console.WriteLine($"Connected to client at {ipEndPoint.Address}:{ipEndPoint.Port}");
//            try
//            {
//                while (!stoppingToken.IsCancellationRequested)
//                {
//                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, stoppingToken).ConfigureAwait(false);
//                    if (bytesRead == 0) break; // Client disconnected

//                    for (int offset = 0; offset < bytesRead; offset += messageLength)
//                    {
//                        if (offset + messageLength <= bytesRead)
//                        {
//                            var messageBytes = new byte[messageLength];
//                            Array.Copy(buffer, offset, messageBytes, 0, messageLength);
//                            var message = Encoding.UTF8.GetString(messageBytes);


//                            // Send message to NServiceBus queue
//                            var nserviceBusMessage = new CommandMessage { Content = message };
//                            lines.Add(message);

//                            await _endpointInstance.SendLocal(nserviceBusMessage).ConfigureAwait(true);
//                            Task.Delay(100).Wait();


//                        }
//                        else
//                        {
//                            // Handle partial message (unlikely but should be handled)
//                            var remainingBytes = bytesRead - offset;
//                            var partialMessageBytes = new byte[remainingBytes];
//                            Array.Copy(buffer, offset, partialMessageBytes, 0, remainingBytes);
//                            // Process partial message if needed
//                        }
//                    }
//                }
//            }
//            catch (OperationCanceledException ex)
//            {
//                Console.WriteLine($"Exception: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception: {ex.Message}");
//            }
//            finally
//            {
//                Console.WriteLine($"Client at {ipEndPoint.Address}:{ipEndPoint.Port} disconnected");
//                client.Close();
//            }
//        }
//    }

//    
//}
