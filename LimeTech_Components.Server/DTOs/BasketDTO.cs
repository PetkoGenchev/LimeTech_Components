namespace LimeTech_Components.Server.DTOs
{
    public class BasketDto
    {
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    }
}
