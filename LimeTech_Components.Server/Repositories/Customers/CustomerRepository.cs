﻿namespace LimeTech_Components.Server.Repositories.Customers
{
    using AutoMapper;
    using LimeTech_Components.Server.Data;
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.DTOs;
    using Microsoft.EntityFrameworkCore;
    using IConfigurationProvider = AutoMapper.IConfigurationProvider;
    public class CustomerRepository : ICustomerRepository
    {

        private readonly LimeTechDbContext _context;
        private readonly IConfigurationProvider mapper;

        public CustomerRepository(LimeTechDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper.ConfigurationProvider;
        }


        public async Task<bool> AddComponentToBasketAsync(string customerId, int componentId)
        {
            var customer = await _context.Customers
                .Include(c => c.BasketItems)
                .ThenInclude(b => b.Component)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return false;
            }

            var basketItem = customer.BasketItems.FirstOrDefault(b => b.Component.Id == componentId);

            if (basketItem != null)
            {
                basketItem.Quantity++;
            }
            else
            {
                var component = await _context.Components.FindAsync(componentId);
                if (component == null)
                {
                    return false;
                }

                customer.BasketItems.Add(new BasketItem
                {
                    Component = component,
                    CustomerId = customer.CustomerId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<BasketItemDTO>> GetBasketAsync(string customerId)
        {
            return await _context.BasketItems
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Component)
                .Select(b => new BasketItemDTO
                {
                    ComponentId = b.Component.Id,
                    ComponentName = b.Component.Name,
                    Quantity = b.Quantity,
                    PricePerUnit = b.Component.Price
                })
                .ToListAsync();
        }




        public async Task<bool> RemoveFromBasketAsync(string customerId, int componentId)
        {
            var item = await _context.BasketItems
                .FirstOrDefaultAsync(b => b.CustomerId == customerId && b.ComponentId == componentId);

            if (item == null) return false;

            _context.BasketItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task AddToPurchaseHistoryAsync(List<PurchaseHistory> purchases)
        {
            await _context.PurchaseHistories.AddRangeAsync(purchases);
            await _context.SaveChangesAsync();
        }


        public async Task ClearBasketAsync(string customerId)
        {
            var basketItems = _context.BasketItems.Where(b => b.CustomerId == customerId);
            _context.BasketItems.RemoveRange(basketItems);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PurchaseHistory>> GetPurchaseHistoryAsync(string customerId)
        {
            return await _context.PurchaseHistories
                .Where(p => p.CustomerId == customerId)
                .Include(p => p.Component)
                .ToListAsync();
        }


    }
}
