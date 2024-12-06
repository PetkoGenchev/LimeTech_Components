namespace LimeTech_Components.Server.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<bool> AddComponentToBasketAsync(string customerId, int componentId);
    }
}
