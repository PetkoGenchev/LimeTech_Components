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



        public async Task<ComponentQueryServiceModel> SearchAndFilterComponentsAsync(
            string? keyword,
            string? name,
            string? producer,
            string? typeOfProduct,
            int? minPrice,
            int? maxPrice,
            int? productionYear,
            PartStatus? status,
            int currentPage = 1,
            int componentsPerPage = ComponentConstants.ComponentsPerPage)
        {
            var query = _context.Components.AsQueryable();


            // Keyword search (matches at least one word, two if multiple words are given)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var words = keyword.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(c => words.Count(w =>
                    c.Name.ToLower().Contains(w) ||
                    c.Producer.ToLower().Contains(w) ||
                    c.TypeOfProduct.ToLower().Contains(w) ||
                    c.ProductionYear.ToString().Contains(w)) >= Math.Min(2, words.Length));
            }


            // Additional filters
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrWhiteSpace(producer))
                query = query.Where(c => c.Producer.ToLower().Contains(producer.ToLower()));

            if (!string.IsNullOrWhiteSpace(typeOfProduct))
                query = query.Where(c => c.TypeOfProduct.ToLower().Contains(typeOfProduct.ToLower()));

            if (minPrice.HasValue)
                query = query.Where(c => c.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(c => c.Price <= maxPrice.Value);

            if (productionYear.HasValue)
                query = query.Where(c => c.ProductionYear == productionYear.Value);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);


            // Sorting and pagination
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.ProductionYear)
                .Skip((currentPage - 1) * componentsPerPage)
                .Take(componentsPerPage)
                .Select(c => new ComponentServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Producer = c.Producer,
                    TypeOfProduct = c.TypeOfProduct,
                    ProductionYear = c.ProductionYear,
                    ImageUrl = c.ImageUrl,
                    Status = c.Status,
                    Price = c.Price,
                    PowerUsage = c.PowerUsage
                })
                .ToListAsync();

            return new ComponentQueryServiceModel
            {
                Components = items,
                TotalComponents = totalItems,
                CurrentPage = currentPage,
                ComponentsPerPage = componentsPerPage
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
    }
}
