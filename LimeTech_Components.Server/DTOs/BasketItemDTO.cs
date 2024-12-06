namespace LimeTech_Components.Server.DTOs
{
    public class BasketItemDto
    {
        public int ComponentId { get; set; }

        public string ComponentName { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
