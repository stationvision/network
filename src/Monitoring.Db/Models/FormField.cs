using Monitoring.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

public class FormField : IEntity
{
    [Key]
    public int FormFieldId { get; set; }

    public int FormId { get; set; }
    public Form Form { get; set; }

    public int FieldId { get; set; }
    public Field Field { get; set; }

    public ICollection<FieldValue> FieldValues { get; set; }
}