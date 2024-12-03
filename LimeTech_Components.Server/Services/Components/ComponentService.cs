namespace LimeTech_Components.Server.Services.Components
{
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.Repositories.Components;
    using LimeTech_Components.Server.Services.Components.Models;
    using System.Linq;
    using static LimeTech_Components.Server.Constants.DataConstants;

    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _componentRepository;

        public ComponentService(IComponentRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }

        public async Task<ComponentQueryServiceModel> GetComponentsAsync(
            int currentPage = 1,
            int componentsPerPage = ComponentConstants.ComponentsPerPage,
            bool publicOnly = true)
        {
            return await _componentRepository.GetComponentsAsync(currentPage, componentsPerPage,publicOnly);
        }

        public async Task<ComponentQueryServiceModel> GetComponentsAsync(
            string name,
            string typeOfProduct,
            int? minPrice,
            int? maxPrice,
            int? productionYear,
            PartStatus? status,
            int currentPage = 1,
            int componentsPerPage = ComponentConstants.ComponentsPerPage)
        {
            return await _componentRepository.GetComponentsAsync(
                name, 
                typeOfProduct, 
                minPrice, 
                maxPrice, 
                productionYear, 
                status,
                currentPage,
                componentsPerPage);
        }

        public async Task<IEnumerable<Component>> GetTopDiscountedComponentsAsync(int top)
        {
            return await _componentRepository.GetTopDiscountedComponentsAsync(top);
        }

        public async Task AddComponentAsync(ComponentServiceModel componentServiceModel)
        {
            // Add business logic
            var component = new Component
            {
                Name = componentServiceModel.Name,
                TypeOfProduct = componentServiceModel.TypeOfProduct,
                ImageUrl = componentServiceModel.ImageUrl,
                Price = componentServiceModel.Price,
                DiscountedPrice = componentServiceModel.DiscountedPrice,
                ProductionYear = componentServiceModel.ProductionYear,
                PowerUsage = componentServiceModel.PowerUsage,
                Status = componentServiceModel.Status,
                StockCount = componentServiceModel.StockCount,
                IsPublic = componentServiceModel.IsPublic,
            };

            await _componentRepository.AddComponentAsync(component);
        }

        public async Task EditComponentAsync(Component component)
        {
            // Business validation, e.g., check if component exists
            var existingComponent = await _componentRepository.GetComponentsAsync(
                component.Name, 
                component.TypeOfProduct, 
                null, 
                null, 
                component.ProductionYear, 
                component.Status,
                1,
                ComponentConstants.ComponentsPerPage);

            if (existingComponent == null)
            {
                throw new KeyNotFoundException("Component not found.");
            }

            await _componentRepository.EditComponentAsync(component);
        }

        public async Task ChangeComponentVisibilityAsync(int componentId, bool isVisible)
        {
            await _componentRepository.ChangeComponentVisibilityAsync(componentId, isVisible);
        }
    }
}
