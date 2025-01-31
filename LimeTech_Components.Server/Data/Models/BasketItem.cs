namespace LimeTech_Components.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class BasketItem
    {
        public int Id { get; set; }


        public int ComponentId { get; set; }

        [Required]
        public Component Component { get; set; } = null!;

        public string CustomerId { get; set; }

        [Required]
        public Customer Customer { get; set; } = null!;


        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

    }
}