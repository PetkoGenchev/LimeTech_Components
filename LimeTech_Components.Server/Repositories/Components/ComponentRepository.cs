using LimeTech_Components.Server.Data;
using LimeTech_Components.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LimeTech_Components.Server.Repositories.Components
{
    public class ComponentRepository : IComponentRepository
    {

        private readonly LimeTechDbContext _context;

        public ComponentRepository(LimeTechDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Component>> GetComponentsAsync(string name, string typeOfProduct, int? minPrice, int? maxPrice, int? productionYear, PartStatus? status)
        {
            var query = _context.Components.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrEmpty(typeOfProduct))
                query = query.Where(c => c.TypeOfProduct == typeOfProduct);

            if (minPrice.HasValue)
                query = query.Where(c => c.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(c => c.Price <= maxPrice.Value);

            if (productionYear.HasValue)
                query = query.Where(c => c.ProductionYear == productionYear.Value);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<Component>> GetTopDiscountedComponentsAsync(int top)
        {
            return await _context.Components
                .OrderByDescending(c => c.DiscountMonth.DiscountPercentage)
                .Take(top)
                .ToListAsync();
        }


        public async Task AddComponentAsync(Component component)
        {
            await _context.Components.AddAsync(component);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateComponentAsync(Component component)
        {
            _context.Components.Update(component);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateComponentVisibilityAsync(int componentId, bool isVisible)
        {
            var component = await _context.Components.FindAsync(componentId);
            if (component != null)
            {
                component.Status = isVisible ? PartStatus.Available : PartStatus.Sold;
                await _context.SaveChangesAsync();
            }
        }
    }
}
