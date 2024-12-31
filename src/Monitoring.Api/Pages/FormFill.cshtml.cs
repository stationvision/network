using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Monitoring.Db.Interfaces;

public class FormFillModel : PageModel
{
    private readonly IRepository<Form> _formrepository;
    private readonly IRepository<Field> _fieldrepository;
    private readonly IRepository<FormField> _formfieldrepository;
    private readonly IRepository<FieldValue> _fieldvaluerepository;

    public FormFillModel(IRepository<Form> formrepository,
        IRepository<Field> fieldrepository,
        IRepository<FormField> formfieldrepository,
        IRepository<FieldValue> fieldvaluerepository)
    {
        _formrepository = formrepository;
        _fieldrepository = fieldrepository;
        _formfieldrepository = formfieldrepository;
        _fieldvaluerepository = fieldvaluerepository;
    }

    [BindProperty]
    public Form Form { get; set; }
    public List<Field> Fields { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Form = _formrepository.Query(x => x.FormId == id).FirstOrDefault();
        if (Form == null)
        {
            return NotFound();
        }

        Fields = _formfieldrepository.Query(x => x.FormId == id).Select(x => x.Field).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var form = _formrepository.Query(x => x.FormId == id).FirstOrDefault();
        if (form == null)
        {
            return NotFound();
        }

        var fields = _formfieldrepository.Query(x => x.FormId == id).Select(x => x.Field).ToList();

        foreach (var field in fields)
        {
            var fieldValue = new FieldValue
            {
                FormFieldId = _formfieldrepository.Query(ff => ff.FormId == id && ff.FieldId == field.FieldId).First().FormFieldId,
                Value = Request.Form["field_" + field.FieldId]
            };
            _fieldvaluerepository.Add(fieldValue);
        }

        _fieldvaluerepository.SaveChanges();
        return RedirectToPage("/Forms");
    }
}
