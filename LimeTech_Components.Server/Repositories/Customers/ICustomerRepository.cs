using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.DTOs;

namespace LimeTech_Components.Server.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<bool> AddComponentToBasketAsync(string customerId, int componentId);

        Task<List<BasketItem>> GetBasketAsync(string customerId);

        Task<bool> RemoveFromBasketAsync(string customerId, List<int> componentIds);

        Task AddToPurchaseHistoryAsync(List<PurchaseHistory> purchases);

        Task<List<PurchaseHistory>> GetPurchaseHistoryAsync(string customerId);
    }
}
