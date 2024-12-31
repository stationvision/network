using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Monitoring.Db.Models
{
    public class Puls : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int PulseTypeId { get; set; }
        public PulseType PulseType { get; set; }

        // Foreign key to Board
        [ForeignKey("Board")]
        public string BoardId { get; set; }

        [JsonIgnore]
        public Board Board { get; set; }

        public int PulsenatureId { get; set; }
        public Pulsnature Nature { get; set; }
        public int NumberofTrackingRange { get; set; }
        public string TrackingRange { get; set; } //JsonData, every range has a name, starter range,End range and a ColorCode
        public int IgnoredDurationThreshold { get; set; } //seconds
        public int IgnoredDifferenceThreshold { get; set; }
        public DateTime Timestamp { get; set; }


    }

    public class PulseType : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Pulsnature : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
