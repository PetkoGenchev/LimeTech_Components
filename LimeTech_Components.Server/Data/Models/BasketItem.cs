namespace LimeTech_Components.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class BasketItem
    {
        public int Id { get; set; }

        public int ComponentID { get; set; }

        [Required]
        public Component Component { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public string CustomerId { get; set; }

        [Required]
        public Customer Customer { get; set; }
    }
}
