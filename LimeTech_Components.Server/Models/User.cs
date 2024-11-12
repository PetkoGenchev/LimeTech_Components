namespace LimeTech_Components.Server.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using static Constants.DataConstants;

    public class User : IdentityUser
    {
        public string PublicID { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(UserProfile.FullNameMaxLength)]
        public string FullName { get; set; }

        public IEnumerable<Component> ComponentBasket { get; init; } = new List<Component>();
    }
}
