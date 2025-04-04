using LimeTech_Components.Server.DTOs;

namespace LimeTech_Components.Server.Services.Customers
{
    public interface ICustomerService
    {
        Task<bool> AddComponentToBasketAsync(string customerId, int componentId);
        Task<List<BasketItemDTO>> GetBasketAsync(string customerId);
        Task<bool> RemoveFromBasketAsync(string customerId, List<int>componentIds);
        Task<(bool success, List<PurchaseHistoryDTO> purchasedItems, decimal totalCost)> PurchaseSelectedItemsAsync(string customerId, List<int> componentIds, Dictionary<int, int> quantities);
        Task<List<PurchaseHistoryDTO>> GetPurchaseHistoryAsync(string customerId);
    }
}
