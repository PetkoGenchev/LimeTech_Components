using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.DTOs;

namespace LimeTech_Components.Server.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<bool> AddComponentToBasketAsync(string customerId, int componentId);

        Task<List<BasketItemDTO>> GetBasketAsync(string customerId);

        Task<bool> RemoveFromBasketAsync(string customerId, int componentId);

        Task AddToPurchaseHistoryAsync(List<PurchaseHistory> purchases);

        Task ClearBasketAsync(string customerId);

        Task<List<PurchaseHistory>> GetPurchaseHistoryAsync(string customerId);
    }
}
