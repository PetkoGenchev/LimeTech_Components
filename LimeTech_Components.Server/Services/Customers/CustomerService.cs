namespace LimeTech_Components.Server.Services.Customers
{
    using LimeTech_Components.Server.Repositories.Customers;

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        public async Task<bool> AddComponentToBasketAsync(string customerId, int componentId)
        {
            return await _customerRepository.AddComponentToBasketAsync(customerId, componentId);
        }


    }
}
