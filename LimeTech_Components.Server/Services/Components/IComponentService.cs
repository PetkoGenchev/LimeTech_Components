using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.DTOs;
using LimeTech_Components.Server.Services.Components.Models;

namespace LimeTech_Components.Server.Services.Components
{
    public interface IComponentService
    {

        //Task<ComponentQueryServiceModel> GetComponentsAsync(
        //    int currentPage,
        //    int componentsPerPage,
        //    bool publicOnly);

        Task<ComponentQueryServiceModel> SearchAndFilterComponentsAsync(
            string? keyword,
            string? name,
            string? producer,
            string? typeOfProduct,
            int? minPrice,
            int? maxPrice,
            int? productionYear,
            PartStatus? status,
            int currentPage,
            int componentsPerPage);


        Task<IEnumerable<ComponentDTO>> GetTopPurchasedComponentsAsync();

        Task<IEnumerable<ComponentDTO>> GetAllComponentsAsync(string sortBy);

        Task AddComponentAsync(ComponentServiceModel component);

        Task<bool> EditComponentAsync(ComponentServiceModel component);

        Task<bool> ChangeComponentVisibilityAsync(int id, bool isVisible);

        Task<IEnumerable<ComponentDTO>> GetAllComponentsSortedByYearAsync();

    }
}
