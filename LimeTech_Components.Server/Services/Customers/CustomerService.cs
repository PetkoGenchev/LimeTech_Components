namespace LimeTech_Components.Server.Services.Customers
{
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.DTOs;
    using LimeTech_Components.Server.Repositories.Customers;
    using AutoMapper;

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }


        public async Task<bool> AddComponentToBasketAsync(string customerId, int componentId)
        {
            return await _customerRepository.AddComponentToBasketAsync(customerId, componentId);
        }


        public async Task<List<BasketItemDto>> GetBasketAsync(string customerId)
        {
            var basket = await _customerRepository.GetBasketAsync(customerId);
            return _mapper.Map<List<BasketItemDto>>(basket);
        }


        public async Task<bool> RemoveFromBasketAsync(string customerId, int componentId)
        {
            return await _customerRepository.RemoveFromBasketAsync(customerId, componentId);
        }


        public async Task<bool> PurchaseBasketAsync(string customerId)
        {
            var basket = await _customerRepository.GetBasketAsync(customerId);
            if (basket == null || !basket.Any()) return false;

            var purchaseHistory = basket.Select(item => new PurchaseHistory
            {
                CustomerId = customerId,
                ComponentId = item.ComponentId,
                ComponentName = item.Component.Name,
                Producer = item.Component.Producer,
                Quantity = item.Quantity,
                TotalPrice = item.Quantity * item.Component.Price,
                PurchaseDate = DateTime.UtcNow
            }).ToList();

            await _customerRepository.AddToPurchaseHistoryAsync(purchaseHistory);
            await _customerRepository.ClearBasketAsync(customerId);

            return true;
        }

    }
}
