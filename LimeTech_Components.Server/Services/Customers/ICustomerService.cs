namespace LimeTech_Components.Server.Services.Customers
{
    public interface ICustomerService
    {
        Task<bool> AddComponentToBasketAsync(string customerId, int componentId);
    }
}
