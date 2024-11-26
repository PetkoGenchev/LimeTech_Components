using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.Services.Components.Models;

namespace LimeTech_Components.Server.Services.Components
{
    public interface IComponentService
    {

        Task<ComponentQueryServiceModel> GetComponentsAsync(
            int currentPage,
            int componentsPerPage,
            bool publicOnly);

        Task<ComponentQueryServiceModel> GetComponentsAsync(
            string name,
            string typeOfProduct,
            int? minPrice,
            int? maxPrice,
            int? productionYear,
            PartStatus? status,
            int currentPage,
            int componentsPerPage);


        Task<IEnumerable<Component>> GetTopDiscountedComponentsAsync(int top);

        Task CreateComponentAsync(Component component);

        Task EditComponentAsync(Component component);

        Task ChangeComponentVisibilityAsync(int componentId, bool isVisible);
    }
}
