using System.ComponentModel.DataAnnotations;
using static LimeTech_Components.Server.Constants.DataConstants;

namespace LimeTech_Components.Server.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(UserProfile.PasswordMinLength)]
        public required string Password { get; set; }

        [Required]
        [MaxLength(UserProfile.FullNameMaxLength)]
        public required string FullName { get; set; }
    }
}
