using LimeTech_Components.Server.DTOs;

namespace LimeTech_Components.Server.Services.Customers
{
    public interface ICustomerService
    {
        Task<bool> AddComponentToBasketAsync(string customerId, int componentId);
        Task<List<BasketItemDTO>> GetBasketAsync(string customerId);
        Task<bool> RemoveFromBasketAsync(string customerId, int componentId);
        Task<bool> PurchaseBasketAsync(string customerId);
        Task<List<PurchaseHistoryDTO>> GetPurchaseHistoryAsync(string customerId);
    }
}
