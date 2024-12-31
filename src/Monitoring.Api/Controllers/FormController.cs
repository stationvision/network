using Microsoft.AspNetCore.Mvc;
using Monitoring.Db.Interfaces;

namespace Monitoring.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[Action]")]
    public class FormController : Controller
    {
        private readonly IRepository<Form> _formrepository;
        private readonly IRepository<Field> _fieldrepository;
        private readonly IRepository<FormField> _formfieldrepository;

        public FormController(IRepository<Form> formrepository,
      IRepository<Field> fieldrepository,
      IRepository<FormField> formfieldrepository)
        {
            _formrepository = formrepository;
            _fieldrepository = fieldrepository;
            _formfieldrepository = formfieldrepository;
        }
        public class FormFieldDto
        {
            public string Type { get; set; }
            public string Label { get; set; }
            public string Extention { get; set; }
        }
        [HttpGet]
        public IActionResult Index()
        {
            var forms = _formrepository.GetAll();
            return Ok(forms);
        }

        [HttpPost]
        public async Task<IActionResult> SaveForm([FromBody] List<FormFieldDto> formStructure)
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
                    Type = fieldDto.Type,
                    Extension = fieldDto.Extention
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
}
