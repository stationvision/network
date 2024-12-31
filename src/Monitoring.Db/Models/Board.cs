using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Monitoring.Db.Models
{
    public class Board : IEntity
    {
        [Key]
        public string Id { get; set; }
        public string IpAddress { get; set; }
        public int NumberOfPulses { get; set; }
        public ICollection<Puls> Pulses { get; set; }
        public ICollection<ClientPuls> ClientPulses { get; set; }
        public ICollection<Machine> machines { get; set; }


    }
}
