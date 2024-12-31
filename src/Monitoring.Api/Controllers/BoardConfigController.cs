using Microsoft.AspNetCore.Mvc;
using Monitoring.Core.Commands.TCPMessages;
using Monitoring.Core.Dispatchers.Interfaces;

namespace Monitoring.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BoardConfigController : ControllerBase
    {

        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public BoardConfigController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        [HttpPost]
        public IActionResult ChangeBoardStatus([FromBody] BoardChangeStatusCommand command)
        {
            _commandDispatcher.Dispatch(command);

            return Ok();
        }
        [HttpPost]
        public IActionResult SendTimeOut([FromBody] TimeOutCommand command)
        {
            _commandDispatcher.Dispatch(command);
            return Ok();
        }
        [HttpPost]
        public IActionResult ConfigBoard([FromBody] SendMessageToConfigBoardCommand command)
        {
            if (command == null)
            {
                return StatusCode(400);
            }
            _commandDispatcher.Dispatch(command);
            return Ok();
        }
    }
}
