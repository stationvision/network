using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Monitoring.Db.IdentityModels;

namespace Monitoring.Api.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")] // Restrict access to Admin role
    public class ViewClaimModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ViewClaimModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser CurrentUser { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
        public IList<string> Roles { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            Claims = await _userManager.GetClaimsAsync(CurrentUser);
            Roles = await _userManager.GetRolesAsync(CurrentUser);
        }
    }
}
