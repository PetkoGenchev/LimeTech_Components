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
            int componentsPerPage = ComponentConstants.ComponentsPerPage)
        {
            var componentQuery = this._context.Components;
            var totalComponents = await componentQuery.CountAsync();

            var components = await GetComponents(componentQuery
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
            string name, 
            string typeOfProduct, 
            int? minPrice, 
            int? maxPrice, 
            int? productionYear, 
            PartStatus? status)
        {
            //var query = _context.Components.AsQueryable();

            //if (!string.IsNullOrEmpty(name))
            //    query = query.Where(c => c.Name.Contains(name));

            //if (!string.IsNullOrEmpty(typeOfProduct))
            //    query = query.Where(c => c.TypeOfProduct == typeOfProduct);

            //if (minPrice.HasValue)
            //    query = query.Where(c => c.Price >= minPrice.Value);

            //if (maxPrice.HasValue)
            //    query = query.Where(c => c.Price <= maxPrice.Value);

            //if (productionYear.HasValue)
            //    query = query.Where(c => c.ProductionYear == productionYear.Value);

            //if (status.HasValue)
            //    query = query.Where(c => c.Status == status.Value);

            var componentModel = new ComponentQueryServiceModel
            {

            };

            return componentModel;
        }


        public async Task<IEnumerable<Component>> GetTopDiscountedComponentsAsync(int top)
        {
            return await _context.Components
                .OrderByDescending(c => c.DiscountMonth.DiscountPercentage)
                .Take(top)
                .ToListAsync();
        }


        public async Task CreateComponentAsync(Component component)
        {
            await _context.Components.AddAsync(component);
            await _context.SaveChangesAsync();
        }

        public async Task EditComponentAsync(Component component)
        {
            _context.Components.Update(component);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeComponentVisibilityAsync(int componentId, bool isVisible)
        {
            var component = await _context.Components.FindAsync(componentId);
            if (component != null)
            {
                component.Status = isVisible ? PartStatus.Available : PartStatus.Sold;
                await _context.SaveChangesAsync();
            }
        }


        private async Task<IEnumerable<ComponentServiceModel>> GetComponents(IQueryable<Component> componentQuery)
            => componentQuery
                .ProjectTo<ComponentServiceModel>(this.mapper)
                .ToList();
    }
}
