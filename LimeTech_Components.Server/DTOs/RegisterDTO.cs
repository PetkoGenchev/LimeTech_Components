using System.ComponentModel.DataAnnotations;
using static LimeTech_Components.Server.Constants.DataConstants;

namespace LimeTech_Components.Server.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(UserProfile.PasswordMinLength)]
        public string Password { get; set; }
    }
}
