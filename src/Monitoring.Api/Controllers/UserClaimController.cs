using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Monitoring.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]/[Action]")]
    public class UserClaimController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
