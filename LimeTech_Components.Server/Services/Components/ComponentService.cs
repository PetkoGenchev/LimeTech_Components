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
            return await _componentRepository.GetComponentsAsync(
                keyword,
                name, 
                producer,
                typeOfProduct, 
                minPrice, 
                maxPrice, 
                productionYear, 
                status,
                currentPage,
                componentsPerPage);
        }


        public async Task<IEnumerable<Component>> GetTopPurchasedComponentsAsync()
        {
            return await _componentRepository.GetTopPurchasedComponentsAsync();
        }


        public async Task<IEnumerable<Component>> GetAllComponentsAsync(string sortBy = null)
        {
            return await _componentRepository.GetAllComponentsAsync(sortBy);
        }


        public async Task AddComponentAsync(ComponentServiceModel componentServiceModel)
        {
            // Add business logic
            var component = new Component
            {
                Name = componentServiceModel.Name,
                Producer = componentServiceModel.Producer,
                TypeOfProduct = componentServiceModel.TypeOfProduct,
                ImageUrl = componentServiceModel.ImageUrl,
                Price = componentServiceModel.Price,
                PurchasedCount = componentServiceModel.PurchasedCount,
                ProductionYear = componentServiceModel.ProductionYear,
                PowerUsage = componentServiceModel.PowerUsage,
                Status = componentServiceModel.Status,
                StockCount = componentServiceModel.StockCount,
                IsPublic = componentServiceModel.IsPublic,
            };

            await _componentRepository.AddComponentAsync(component);
        }

        public async Task<bool> EditComponentAsync(ComponentServiceModel componentServiceModel)
        {
            var existingComponent = await _componentRepository.GetComponentByIdAsync(componentServiceModel.Id);

            if (existingComponent == null)
            {
                return false;
            }

            existingComponent.Name = componentServiceModel.Name;
            existingComponent.Producer = componentServiceModel.Producer;
            existingComponent.TypeOfProduct = componentServiceModel.TypeOfProduct;
            existingComponent.ImageUrl = componentServiceModel.ImageUrl;
            existingComponent.Price = componentServiceModel.Price;
            existingComponent.PurchasedCount = componentServiceModel.PurchasedCount;
            existingComponent.ProductionYear = componentServiceModel.ProductionYear;
            existingComponent.PowerUsage = componentServiceModel.PowerUsage;
            existingComponent.Status = componentServiceModel.Status;
            existingComponent.StockCount = componentServiceModel.StockCount;
            existingComponent.IsPublic = componentServiceModel.IsPublic;

            await _componentRepository.UpdateComponentAsync(existingComponent);

            return true;
        }

        public async Task<bool> ChangeComponentVisibilityAsync(int id, bool isVisible)
        {
            var component = await _componentRepository.GetComponentByIdAsync(id);

            if (component == null)
            {
                return false;
            }
            component.IsPublic = isVisible;

            await _componentRepository.UpdateComponentAsync(component);

            return true;
        }
    }
}
