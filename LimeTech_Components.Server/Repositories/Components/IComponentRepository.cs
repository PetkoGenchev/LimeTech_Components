using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.DTOs;
using LimeTech_Components.Server.Services.Components.Models;
using System.Collections.Generic;
using static LimeTech_Components.Server.Constants.DataConstants;

namespace LimeTech_Components.Server.Repositories.Components
{
    public interface IComponentRepository
    {

        Task<ComponentQueryServiceModel> GetComponentsAsync(
            int currentPage,
            int componentsPerPage,
            bool publicOnly);

        Task<ComponentQueryServiceModel> GetComponentsAsync(
            string keyword,
            string name, 
            string producer, 
            string typeOfProduct, 
            int? minPrice, 
            int? maxPrice, 
            int? productionYear,
            PartStatus? status,
            int currentPage,
            int componentsPerPage);



        Task<IEnumerable<Component>> GetTopPurchasedComponentsAsync();

        Task AddComponentAsync(Component component);

        Task EditComponentAsync(Component component);

        Task<Component> GetComponentByIdAsync(int id);

        Task UpdateComponentAsync(Component component);

        Task<IEnumerable<Component>> GetAllComponentsAsync(string sortBy);

        Task<IEnumerable<Component>> GetAllComponentsSortedByYearAsync();

        Task<IEnumerable<Component>> SearchComponentsAsync(string query);
    }
}
