namespace LimeTech_Components.Server.Repositories.Components
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LimeTech_Components.Server.Data;
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.DTOs;
    using LimeTech_Components.Server.Services.Components.Models;
    using Microsoft.EntityFrameworkCore;
    using static LimeTech_Components.Server.Constants.DataConstants;
    public class ComponentRepository : IComponentRepository
    {

        private readonly LimeTechDbContext _context;
        private readonly IConfigurationProvider mapper;

        public ComponentRepository(LimeTechDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper.ConfigurationProvider;
        }


        public async Task<ComponentQueryServiceModel> GetComponentsAsync(
            int currentPage = 1,
            int componentsPerPage = ComponentConstants.ComponentsPerPage,
            bool publicOnly = false)
        {
            var componentQuery = this._context.Components
                .Where(p => !publicOnly || p.IsPublic)
                .OrderBy(p => p.TypeOfProduct)
                .ThenBy(p => p.Name);

            var totalComponents = await componentQuery.CountAsync();

            var components = await GetComponentsAsync(componentQuery
                .Skip((currentPage - 1) * componentsPerPage)
                .Take(componentsPerPage));

            return new ComponentQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalComponents = totalComponents,
                ComponentsPerPage = componentsPerPage,
                Components = components
            };
        }



        public async Task<ComponentQueryServiceModel> GetComponentsAsync(
            string keyword,
            string name,
            string producer,
            string typeOfProduct,
            int? minPrice,
            int? maxPrice,
            int? productionYear,
            PartStatus? status,
            int currentPage = 1,
            int componentsPerPage = ComponentConstants.ComponentsPerPage)
        {
            var query = _context.Components.AsQueryable();


            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c =>
                    c.Name.Contains(keyword) ||
                    c.TypeOfProduct.Contains(keyword) ||
                    c.ProductionYear.ToString().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(producer))
            {
                query = query.Where(c => c.Producer.Contains(producer));
            }


            if (!string.IsNullOrEmpty(typeOfProduct))
            {
                query = query.Where(t => t.TypeOfProduct == typeOfProduct);
            }


            if (minPrice.HasValue)
            {
                query = query.Where(c => c.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= maxPrice.Value);
            }

            if (productionYear.HasValue)
            {
                query = query.Where(c => c.ProductionYear == productionYear.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status);
            }

            var components = await GetComponentsAsync(query
                .Skip((currentPage - 1) * componentsPerPage)
                .Take(componentsPerPage));

            var totalComponents = components.Count();

            return new ComponentQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalComponents = totalComponents,
                ComponentsPerPage = componentsPerPage,
                Components = components
            };
        }


        public async Task<IEnumerable<Component>> GetTopPurchasedComponentsAsync()
        {
            return await _context.Components
                .OrderByDescending(c => c.PurchasedCount)
                .Take(10)
                .ToListAsync();
        }


        public async Task AddComponentAsync(Component component)
        {
            await _context.Components.AddAsync(component);
            await _context.SaveChangesAsync();
        }

        public async Task EditComponentAsync(Component component)
        {
            _context.Components.Update(component);
            await _context.SaveChangesAsync();
        }

        private async Task<IEnumerable<ComponentServiceModel>> GetComponentsAsync(IQueryable<Component> componentQuery)
            => await componentQuery
                .ProjectTo<ComponentServiceModel>(this.mapper)
                .ToListAsync();


        public async Task<Component> GetComponentByIdAsync(int id)
        {
            return await _context.Components.FindAsync(id);
        }

        public async Task UpdateComponentAsync(Component component)
        {
            _context.Components.Update(component);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Component>> GetAllComponentsAsync(string sortBy = null)
        {
            var query = _context.Components.AsQueryable();

            // Apply sorting
            query = sortBy switch
            {
                "productionYear" => query.OrderBy(c => c.ProductionYear),
                "price" => query.OrderBy(c => c.Price),
                "producer" => query.OrderBy(c => c.Producer),
                "purchasedCount" => query.OrderByDescending(c => c.PurchasedCount),
                _ => query // No sorting applied
            };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Component>> GetAllComponentsSortedByYearAsync()
        {
            return await _context.Components
                .OrderByDescending(c => c.ProductionYear)
                .ToListAsync();
        }

        public async Task<IEnumerable<Component>> SearchComponentsAsync(string query)
        {
            var words = query.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0) return new List<Component>();

            return await _context.Components
                .Where(c => words.Count(w =>
                    c.Name.ToLower().Contains(w) ||
                    c.Producer.ToLower().Contains(w) ||
                    c.TypeOfProduct.ToLower().Contains(w) ||
                    c.ProductionYear.ToString().Contains(w)) >= Math.Min(2, words.Length)) // At least 2 words must match
                .ToListAsync();
        }
    }
}
