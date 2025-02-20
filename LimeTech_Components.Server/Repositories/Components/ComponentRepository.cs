namespace LimeTech_Components.Server.Repositories.Components
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LimeTech_Components.Server.Data;
    using LimeTech_Components.Server.Data.Models;
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
            bool publicOnly = true)
        {
            var componentQuery = this._context.Components.Where(p => !publicOnly || p.IsPublic);

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
    }
}
