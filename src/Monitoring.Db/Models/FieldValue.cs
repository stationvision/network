using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

public class FieldValue : IEntity
{
    [Key]
    public int FieldValueId { get; set; }

    public int FormFieldId { get; set; }
    public FormField FormField { get; set; }

    public string Value { get; set; }
    public DateTime SubmittedDate { get; set; } = DateTime.Now;
}