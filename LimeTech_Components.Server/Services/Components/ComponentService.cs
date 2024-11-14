using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.Repositories.Components;

namespace LimeTech_Components.Server.Services.Components
{
    public class ComponentService
    {
        private readonly IComponentRepository _componentRepository;

        public ComponentService(IComponentRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }

        public async Task<IEnumerable<Component>> GetComponentsAsync(
            string name, string typeOfProduct, int? minPrice, int? maxPrice, int? productionYear, PartStatus? status)
        {
            return await _componentRepository.GetComponentsAsync(name, typeOfProduct, minPrice, maxPrice, productionYear, status);
        }

        public async Task<IEnumerable<Component>> GetTopDiscountedComponentsAsync(int top)
        {
            return await _componentRepository.GetTopDiscountedComponentsAsync(top);
        }

        public async Task CreateComponentAsync(Component component)
        {
            // Additional business logic before adding
            await _componentRepository.AddComponentAsync(component);
        }

        public async Task EditComponentAsync(Component component)
        {
            // Business validation, e.g., check if component exists
            var existingComponent = await _componentRepository.GetComponentsAsync(
                component.Name, component.TypeOfProduct, null, null, component.ProductionYear, component.Status);

            if (existingComponent == null)
            {
                throw new KeyNotFoundException("Component not found.");
            }

            await _componentRepository.UpdateComponentAsync(component);
        }

        public async Task ChangeComponentVisibilityAsync(int componentId, bool isVisible)
        {
            await _componentRepository.UpdateComponentVisibilityAsync(componentId, isVisible);
        }
    }
}
