using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.Services.Components.Models;
using System.Collections.Generic;
using static LimeTech_Components.Server.Constants.DataConstants;

namespace LimeTech_Components.Server.Repositories.Components
{
    public interface IComponentRepository
    {

        Task<ComponentQueryServiceModel> GetComponentsAsync(
            int currentPage,
            int componentsPerPage);

        Task<ComponentQueryServiceModel> GetComponentsAsync(
            string name, 
            string typeOfProduct, 
            int? minPrice, 
            int? maxPrice, 
            int? productionYear,
            PartStatus? status,
            int currentPage = 1,
            int componentsPerPage = ComponentConstants.ComponentsPerPage);

        Task<IEnumerable<Component>> GetTopDiscountedComponentsAsync(int top);

        Task CreateComponentAsync(Component component);

        Task EditComponentAsync(Component component);

        Task ChangeComponentVisibilityAsync(int componentId, bool isVisible);
    }
}
