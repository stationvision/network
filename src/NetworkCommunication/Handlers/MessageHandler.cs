using Microsoft.Extensions.Options;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;
using NetworkCommunications.Extentions;
using NetworkCommunications.Options;
using NetworkCommunications.Services;
using System.Globalization;

namespace NetworkCommunications.Handlers
{
    public class MessageHandler : IHandleMessages<CommandMessage>
    {
        private readonly IRepository<MessageEntity> _repository;
        private readonly MessageProcessingService _messageProcessingService;
        private readonly IOptions<ConfigOption> _options;

        public MessageHandler(IRepository<MessageEntity> repository, MessageProcessingService messageProcessingService, IOptions<ConfigOption> options)
        {
            _repository = repository;
            _messageProcessingService = messageProcessingService;
            _options = options;
        }

        public Task Handle(CommandMessage message, IMessageHandlerContext context)
        {
            try
            {
                var ArrayContent = message.Content.Split(",");
                var messageType = ArrayContent.DetermineMessageType();

                string pulsDate = ArrayContent.GetPulsDate();
                DateTime parsedDate = DateTime.ParseExact(pulsDate, "MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture);


                var messageEntity = new MessageEntity
                {
                    Content = message.Content,
                    ErrorDetails = "",
                    //MessageId = context.MessageId,
                    MessageId = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.Now,
                    Status = Enums.MessageStatus.Pending,
                    MessageType = messageType.ToString(),
                    BoardId = ArrayContent.GetBoardId(),
                    PulsData = parsedDate
                };

                _repository.Add(messageEntity);
                _repository.SaveChanges();

                // Add message to buffer
                var type = new MessageStoreType() { StartDate = messageEntity.PulsData, MessageId = Guid.Parse(messageEntity.MessageId) };

                _messageProcessingService.AddMessage(messageEntity, type);
                _messageProcessingService.StartAsync(CancellationToken.None);
                Console.WriteLine($"Pulse Date : {messageEntity.PulsData}");
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
        }
    }
}