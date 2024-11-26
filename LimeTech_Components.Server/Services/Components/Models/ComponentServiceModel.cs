using LimeTech_Components.Server.Data.Models;

namespace LimeTech_Components.Server.Services.Components.Models
{
    public class ComponentServiceModel
    {
        public int Id { get; init; }

        public string? TypeOfProduct { get; init; }

        public string? ImageUrl { get; init; }

        public int Price { get; init; }

        public int DiscountedPrice { get; init; }

        public int ProductionYear { get; init; }

        public int PowerUsage { get; init; }

        public PartStatus Status { get; init; }

        public bool IsPublic { get; init; }
    }
}
