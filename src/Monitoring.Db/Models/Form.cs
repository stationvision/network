using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

public class Form : IEntity
{
    [Key]
    public int FormId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public ICollection<FormField> FormFields { get; set; }
}