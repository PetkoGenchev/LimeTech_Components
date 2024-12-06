using System.ComponentModel.DataAnnotations;

namespace LimeTech_Components.Server.Data.Models
{
    public class PurchaseHistory
    {
        public int Id { get; set; }
        public List<Component> Components { get; set; } = new List<Component>();
        public DateTime DateOfPurchase { get; set; }

        public string CustomerId { get; set; }

        [Required]
        public Customer Customer { get; set; }
    }
}
