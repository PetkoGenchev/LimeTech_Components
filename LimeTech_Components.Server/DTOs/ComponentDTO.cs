namespace LimeTech_Components.Server.DTOs
{
    public class ComponentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Producer { get; set; } = null!;
        public string TypeOfProduct { get; set; } = null!;
        public decimal Price { get; set; }
        public int ProductionYear { get; set; }
    }

}
