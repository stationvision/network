using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Monitoring.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleManagementController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            return Ok(_roleManager.Roles);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateModel model)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }
    }

    public class RoleCreateModel
    {
        public string RoleName { get; set; }
    }
}
