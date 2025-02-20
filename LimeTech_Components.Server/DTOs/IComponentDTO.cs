namespace LimeTech_Components.Server.DTOs
{
    using LimeTech_Components.Server.Data.Models;
    public interface IComponentDTO
    {
        string? Name { get; }
        string? Producer { get; }
        string? TypeOfProduct { get; }
        string? ImageUrl { get; }
        int Price { get; }
        int PurchasedCount { get; }
        int ProductionYear { get; }
        int PowerUsage { get; }
        PartStatus Status { get; }
        int StockCount { get; }
        bool IsPublic { get; }
    }
}
