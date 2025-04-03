using LimeTech_Components.Server.Data.Models;

namespace LimeTech_Components.Server.DTOs
{
    public class ComponentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Producer { get; set; } = null!;
        public required string TypeOfProduct { get; set; }
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int ProductionYear { get; set; }
        public int PowerUsage { get; set; }
        public PartStatus Status { get; set; }
    }

}
