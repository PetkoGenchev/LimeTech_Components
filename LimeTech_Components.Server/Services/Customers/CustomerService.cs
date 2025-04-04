namespace LimeTech_Components.Server.Services.Customers
{
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.DTOs;
    using LimeTech_Components.Server.Repositories.Customers;
    using AutoMapper;
    using LimeTech_Components.Server.Repositories.Components;

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IMapper mapper,
            IComponentRepository componentRepository)
        {
            _customerRepository = customerRepository;
            _componentRepository = componentRepository;
            _mapper = mapper;
        }


        public async Task<bool> AddComponentToBasketAsync(string customerId, int componentId)
        {
            return await _customerRepository.AddComponentToBasketAsync(customerId, componentId);
        }


        public async Task<List<BasketItemDTO>> GetBasketAsync(string customerId)
        {
            var basket = await _customerRepository.GetBasketAsync(customerId);
            return _mapper.Map<List<BasketItemDTO>>(basket);
        }


        public async Task<bool> RemoveFromBasketAsync(string customerId, List<int> componentId)
        {
            return await _customerRepository.RemoveFromBasketAsync(customerId, componentId);
        }



        public async Task<(bool success, List<PurchaseHistoryDTO> purchasedItems, decimal totalCost)>
            PurchaseSelectedItemsAsync(string customerId, List<int> componentIds, Dictionary<int, int> quantities)
        {
            var basket = await _customerRepository.GetBasketAsync(customerId);
            if (basket == null || !basket.Any()) return (false, new List<PurchaseHistoryDTO>(), 0);


            var selectedItems = basket.Where(item => componentIds.Contains(item.ComponentId)).ToList();
            if (!selectedItems.Any()) return (false, new List<PurchaseHistoryDTO>(), 0);


            var purchasedItems = new List<PurchaseHistory>();
            decimal totalCost = 0;


            foreach (var item in selectedItems)
            {
                int requestedQuantity = quantities[item.ComponentId];

                var component = await _componentRepository.GetComponentByIdAsync(item.ComponentId);

                if (component == null || component.StockCount < requestedQuantity)
                {
                    return (false, new List<PurchaseHistoryDTO>(), 0);
                }

                component.StockCount -= requestedQuantity;

                if (component.StockCount == 0)
                {
                    component.Status = PartStatus.Sold;
                }

                await _componentRepository.UpdateComponentAsync(component);


                var purchase = new PurchaseHistory
                {
                    CustomerId = customerId,
                    ComponentId = item.ComponentId,
                    ComponentName = component.Name,
                    Producer = component.Producer,
                    Quantity = requestedQuantity,
                    TotalPrice = requestedQuantity * component.Price,
                    PurchaseDate = DateTime.UtcNow
                };

                purchasedItems.Add(purchase);
                totalCost += purchase.TotalPrice;
            }
            await _customerRepository.AddToPurchaseHistoryAsync(purchasedItems);
            await _customerRepository.RemoveFromBasketAsync(customerId, componentIds);


            var purchasedItemsDto = _mapper.Map<List<PurchaseHistoryDTO>>(purchasedItems);

            return (true, purchasedItemsDto, totalCost);
        }


        public async Task<List<PurchaseHistoryDTO>> GetPurchaseHistoryAsync(string customerId)
        {
            var purchaseHistory = await _customerRepository.GetPurchaseHistoryAsync(customerId);
            return _mapper.Map<List<PurchaseHistoryDTO>>(purchaseHistory);
        }


    }
}
