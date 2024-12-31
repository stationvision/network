using Microsoft.AspNetCore.Mvc;
using Monitoring.Core.Commands.Puls;
using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Queries.Puls;

namespace Monitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PulsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PulsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var command = new GetAlPulslQuery();
            var boards = _queryDispatcher.Dispatch(command);

            return StatusCode(200, boards);
        }
        [HttpGet]
        public IActionResult Get(int Id)
        {
            var query = new GetByIdQuery() { Id = Id };
            var boards = _queryDispatcher.Dispatch(query);

            return StatusCode(200, boards);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreatePulsCommand createPulsCommand)
        {
            _commandDispatcher.Dispatch(createPulsCommand);
            return StatusCode(200);
        }
        [HttpPost]
        public IActionResult CreateBatch([FromBody] List<CreatePulsCommand> createPulsCommands)
        {
            if (createPulsCommands == null || createPulsCommands.Count == 0)
            {
                return StatusCode(400);
            }
            foreach (var createPulsCommand in createPulsCommands)
            {
                _commandDispatcher.Dispatch(createPulsCommand);
            }

            return StatusCode(200);
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] DeletePulsCommand deletePulsCommand)
        {
            _commandDispatcher.Dispatch(deletePulsCommand);
            return StatusCode(200);
        }
        [HttpPut]
        public IActionResult Edit([FromBody] EditPulsCommand editPulsCommand)
        {
            _commandDispatcher.Dispatch(editPulsCommand);
            return StatusCode(200);
        }
    }
}