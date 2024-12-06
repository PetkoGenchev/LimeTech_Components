namespace LimeTech_Components.Server.Repositories.Customers
{
    using AutoMapper;
    using LimeTech_Components.Server.Data;
    using LimeTech_Components.Server.Data.Models;
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
                .FirstOrDefaultAsync(c => c.PublicID == customerId);

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
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
