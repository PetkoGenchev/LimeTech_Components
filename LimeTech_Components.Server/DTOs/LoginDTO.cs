using System.ComponentModel.DataAnnotations;

namespace LimeTech_Components.Server.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
