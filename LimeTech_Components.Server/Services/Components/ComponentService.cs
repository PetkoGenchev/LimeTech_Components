namespace LimeTech_Components.Server.Services.Components
{
    using AutoMapper;
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.DTOs;
    using LimeTech_Components.Server.Repositories.Components;
    using LimeTech_Components.Server.Services.Components.Models;
    using System.Globalization;
    using System.Linq;
    using static LimeTech_Components.Server.Constants.DataConstants;

    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _componentRepository;
        private readonly IMapper _mapper;

        public ComponentService(IComponentRepository componentRepository, IMapper mapper)
        {
            _componentRepository = componentRepository;
            _mapper = mapper;
        }

        public async Task<ComponentQueryServiceModel> GetComponentsAsync(
            int currentPage = 1,
            int componentsPerPage = ComponentConstants.ComponentsPerPage,
            bool publicOnly = false)
        {
            return await _componentRepository.GetComponentsAsync(currentPage, componentsPerPage, publicOnly);
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
            return await _componentRepository.SearchAndFilterComponentsAsync(
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


        public async Task<IEnumerable<ComponentDTO>> GetTopPurchasedComponentsAsync()
        {
            var components = await _componentRepository.GetTopPurchasedComponentsAsync();

            return components.Select(c => new ComponentDTO
            {
                Id = c.Id,
                Name = c.Name,
                Producer = c.Producer,
                TypeOfProduct = c.TypeOfProduct,
                ImageUrl = c.ImageUrl,
                Price = c.Price,
                ProductionYear = c.ProductionYear,
                PowerUsage = c.PowerUsage,
                Status = c.Status

            }).ToList();
        }


        public async Task<IEnumerable<ComponentDTO>> GetAllComponentsAsync(string sortBy = null)
        {
            var components = await _componentRepository.GetAllComponentsAsync(sortBy);

            return components.Select(c => new ComponentDTO
            {
                Id = c.Id,
                Name = c.Name,
                Producer = c.Producer,
                TypeOfProduct = c.TypeOfProduct,
                ImageUrl = c.ImageUrl,
                Price = c.Price,
                ProductionYear = c.ProductionYear,
                PowerUsage = c.PowerUsage,
                Status = c.Status

            }).ToList();
        }


        public async Task AddComponentAsync(ComponentServiceModel componentServiceModel)
        {
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

        public async Task<IEnumerable<ComponentDTO>> GetAllComponentsSortedByYearAsync()
        {
            var components = await _componentRepository.GetAllComponentsSortedByYearAsync();

            return components.Select(c => new ComponentDTO
            {
                Id = c.Id,
                Name = c.Name,
                Producer = c.Producer,
                TypeOfProduct = c.TypeOfProduct,
                ImageUrl = c.ImageUrl,
                Price = c.Price,
                ProductionYear = c.ProductionYear,
                PowerUsage = c.PowerUsage,
                Status = c.Status

            }).ToList();
        }
    }
}
