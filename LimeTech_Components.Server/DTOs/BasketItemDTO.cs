namespace LimeTech_Components.Server.DTOs
{
    public class BasketItemDTO
    {
        public int ComponentId { get; set; }

        public string ComponentName { get; set; } = null!;

        public string Producer { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal PricePerUnit { get; set; }

        public decimal TotalPrice => PricePerUnit * Quantity;
    }
}
