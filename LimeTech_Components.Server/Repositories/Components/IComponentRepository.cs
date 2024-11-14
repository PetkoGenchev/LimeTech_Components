using LimeTech_Components.Server.Data.Models;
using System.Collections.Generic;

namespace LimeTech_Components.Server.Repositories.Components
{
    public interface IComponentRepository
    {
        Task<IEnumerable<Component>> GetComponentsAsync(
            string name, string typeOfProduct, int? minPrice, 
            int? maxPrice, int? productionYear,
            PartStatus? status);

        Task<IEnumerable<Component>> GetTopDiscountedComponentsAsync(int top);

        Task AddComponentAsync(Component component);

        Task UpdateComponentAsync(Component component);

        Task UpdateComponentVisibilityAsync(int componentId, bool isVisible);
    }
}
