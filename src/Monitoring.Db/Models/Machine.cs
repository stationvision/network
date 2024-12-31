using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Monitoring.Db.Models
{
    public class Machine : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string BoardId { get; set; }
        public Board Board { get; set; }

    }
}
