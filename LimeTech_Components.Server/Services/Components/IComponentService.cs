using LimeTech_Components.Server.Data.Models;

namespace LimeTech_Components.Server.Services.Components
{
    public interface IComponentService
    {
        Task<IEnumerable<Component>> GetComponentsAsync(
        string name, string typeOfProduct, int? minPrice, int? maxPrice, int? productionYear, PartStatus? status);

        Task<IEnumerable<Component>> GetTopDiscountedComponentsAsync(int top);

        Task CreateComponentAsync(Component component);

        Task EditComponentAsync(Component component);

        Task ChangeComponentVisibilityAsync(int componentId, bool isVisible);
    }
}
