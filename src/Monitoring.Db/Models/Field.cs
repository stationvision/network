using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

public class Field : IEntity
{
    [Key]
    public int FieldId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // e.g., text, number, date
    public string Extension { get; set; } // e.g., cm, kg, or nothing

    public ICollection<FormField> FormFields { get; set; }
}