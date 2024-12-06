namespace LimeTech_Components.Server.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using static Constants.DataConstants;

    public class Customer : IdentityUser
    {
        public string? PublicID { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(UserProfile.FullNameMaxLength)]
        public string? FullName { get; set; }

        public List<PurchaseHistory> PurchaseHistories { get; private set; } = new List<PurchaseHistory>();

        public List<BasketItem> BasketItems { get; private set; } = new List<BasketItem>();
    }
}
