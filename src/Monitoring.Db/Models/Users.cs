using System.ComponentModel.DataAnnotations;

namespace Monitoring.Db.Models
{
    public class Users
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserRoles Role { get; set; }

        public List<UserRoles> Roles { get; set; }
    }

    public enum UserRoles
    {
        Admin,
        User
    }
}
