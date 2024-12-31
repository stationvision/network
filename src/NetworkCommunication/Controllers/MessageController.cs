using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Monitoring.Db;
using NetworkCommunications.Options;
using System.Net.Sockets;
using System.Text;

namespace NetworkCommunications.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ConfigOption _options;
        private readonly IdentityMonitoringDbContext _dbContext;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // Semaphore for locking

        public MessageController(IdentityMonitoringDbContext dbContext, IOptions<ConfigOption> options)
        {
            _options = options.Value;
            _dbContext = dbContext;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(request.IpAddress, request.Port);
                    var stream = client.GetStream();
                    var messageBytes = Encoding.UTF8.GetBytes(request.Message);
                    await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                }

                return Ok("Message sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending message: {ex.Message}");
            }
        }

        [HttpDelete("VanishQueue")]
        public async Task<IActionResult> CleanQueue()
        {
            await _semaphore.WaitAsync(); // Wait to enter the critical section
            try
            {
                using (_dbContext)
                {
                    string tableName = "TcpServiceBusEndpoint";
                    string query = $"DELETE FROM {tableName}";

                    int rowsAffected = _dbContext.Database.ExecuteSqlRaw(query);
                    var result = $"Cleared {rowsAffected} messages from {tableName}.";
                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending message: {ex.Message}");
            }
            finally { _semaphore.Release(); }
        }


    }

    public class SendMessageRequest
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Message { get; set; }
    }

}
