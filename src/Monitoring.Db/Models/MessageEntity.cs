using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using static Monitoring.Db.Interfaces.Enums;

namespace Monitoring.Db.Models
{
    public class MessageEntity : IEntity
    {

        [Key]
        public Int64 Id { get; set; } // This will be auto-incremented
        public string MessageId { get; set; } // This will store the unique message ID from NServiceBus
        public string Content { get; set; } // This will store the actual message content
        public MessageStatus Status { get; set; }
        public string ErrorDetails { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public string BoardId { get; set; }
        public DateTime PulsData { get; set; }
    }


    public enum MessageType
    {
        FromBoardsToServer,
        SettingsFromServerToClient,
        CommandsFromServerToClientTOChangeStatus,
        TimeAndDateFromServerToClient,
        TimeoutSettingFromServerToClient,
        Unknown = 0
    }
}
