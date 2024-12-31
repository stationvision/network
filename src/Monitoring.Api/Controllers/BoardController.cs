using Microsoft.AspNetCore.Mvc;
using Monitoring.Core.Commands.Boards;
using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Queries.Boards;

namespace Monitoring.Api.Controllers
{
    //[ApiController]
    [Route("api/[controller]/[action]")]
    public class BoardController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public BoardController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var boardcommand = new GetAllBoardQuery();
            var boards = _queryDispatcher.Dispatch(boardcommand);

            return Ok(boards);
        }
        [HttpGet]
        public IActionResult Get(string Id)
        {
            var query = new GetBoardByIdQuery() { Id = Id };
            var boards = _queryDispatcher.Dispatch(query);

            return StatusCode(200, boards);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateBoardCommand createBoardCommand)
        {
            if (createBoardCommand == null)
            {
                return StatusCode(400);
            }
            _commandDispatcher.Dispatch(createBoardCommand);
            return StatusCode(200);
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] DeleteBoardCommand createBoardCommand)
        {
            _commandDispatcher.Dispatch(createBoardCommand);
            return StatusCode(200);
        }
        [HttpPut]
        public IActionResult Edit([FromBody] EditBoardCommand editeBoardCommand)
        {
            _commandDispatcher.Dispatch(editeBoardCommand);
            return StatusCode(200);
        }
    }
}
