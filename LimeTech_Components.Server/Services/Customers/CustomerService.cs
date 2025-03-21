﻿namespace LimeTech_Components.Server.Services.Customers
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


        public async Task<List<BasketItemDTO>> GetBasketAsync(string customerId)
        {
            var basket = await _customerRepository.GetBasketAsync(customerId);
            return _mapper.Map<List<BasketItemDTO>>(basket);
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
                ComponentName = item.ComponentName,
                Producer = item.ComponentProducer,
                Quantity = item.Quantity,
                TotalPrice = item.Quantity * item.PricePerUnit,
                PurchaseDate = DateTime.UtcNow
            }).ToList();

            await _customerRepository.AddToPurchaseHistoryAsync(purchaseHistory);
            await _customerRepository.ClearBasketAsync(customerId);

            return true;
        }

        public async Task<List<PurchaseHistoryDTO>> GetPurchaseHistoryAsync(string customerId)
        {
            var purchaseHistory = await _customerRepository.GetPurchaseHistoryAsync(customerId);
            return purchaseHistory.Select(p => new PurchaseHistoryDTO
            {
                PurchaseDate = p.PurchaseDate,
                ComponentName = p.Component.Name,
                Producer = p.Component.Producer,
                Quantity = p.Quantity,
                TotalPrice = p.TotalPrice
            }).ToList();
        }


    }
}
