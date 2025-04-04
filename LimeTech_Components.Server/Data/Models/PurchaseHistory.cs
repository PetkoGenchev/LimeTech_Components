using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimeTech_Components.Server.Data.Models
{
    public class PurchaseHistory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public required string CustomerId { get; set; }

        [Required]
        public Customer Customer { get; set; } = null!;


        public int ComponentId { get; set; }

        [Required]
        public Component Component { get; set; } = null!;

        public string ComponentName { get; set; } = null!;
        public string Producer { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }

    }
}
