using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Monitoring.Db.Interfaces.Enums;

namespace Monitoring.Db.Models
{
    public class ClientPuls : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string BoardId { get; set; }
        public Board Board { get; set; }
        [ForeignKey("PulsId")]
        public int PulsId { get; set; }
        public Puls puls { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
        public string Hash { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PulsStatus status { get; set; }
        public string MessageId { get; set; }
        public double? DeviationTime { get; set; }
        public bool IncreaseProductionTime { get; set; }

    }
}
