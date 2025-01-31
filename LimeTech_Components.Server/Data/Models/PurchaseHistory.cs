using System.ComponentModel.DataAnnotations;

namespace LimeTech_Components.Server.Data.Models
{
    public class PurchaseHistory
    {
        public int Id { get; set; }
        public DateTime DateOfPurchase { get; set; }

        public string CustomerId { get; set; }

        [Required]
        public Customer Customer { get; set; } = null!;


        public int ComponentId { get; set; }

        [Required]
        public Component Component { get; set; } = null!;

    }
}
