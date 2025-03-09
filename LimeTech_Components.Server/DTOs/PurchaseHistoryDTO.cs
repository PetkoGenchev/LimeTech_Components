namespace LimeTech_Components.Server.DTOs
{
    public class PurchaseHistoryDTO
    {
        public string ComponentName { get; set; } = null!;
        public string Producer { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

}
