using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Monitoring.Db.Interfaces;
using static Monitoring.Api.Controllers.FormController;

public class FormBuilderModel : PageModel
{
    private readonly IRepository<Form> _formrepository;
    private readonly IRepository<Field> _fieldrepository;
    private readonly IRepository<FormField> _formfieldrepository;

    public FormBuilderModel(IRepository<Form> formrepository,
        IRepository<Field> fieldrepository,
        IRepository<FormField> formfieldrepository)
    {
        _formrepository = formrepository;
        _fieldrepository = fieldrepository;
        _formfieldrepository = formfieldrepository;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostSaveForm([FromBody] List<FormFieldDto> formStructure)
    {
        if (formStructure == null || formStructure.Count == 0)
        {
            return BadRequest("Form structure is invalid.");
        }

        var form = new Form
        {
            Title = "New Form",
            Description = "Form description"
        };

        _formrepository.Add(form);
        _formrepository.SaveChanges();

        foreach (var fieldDto in formStructure)
        {
            var field = new Field
            {
                Name = fieldDto.Label,
                Type = fieldDto.Type
            };
            _fieldrepository.Add(field);
            _fieldrepository.SaveChanges();

            var formField = new FormField
            {
                FormId = form.FormId,
                FieldId = field.FieldId
            };
            _formfieldrepository.Add(formField);
        }

        _formfieldrepository.SaveChanges();
        return new JsonResult(new { success = true });
    }
}


