using LimeTech_Components.Server.Data.Models;
using static LimeTech_Components.Server.Constants.DataConstants;

namespace LimeTech_Components.Server.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("components")]
        public async Task<IActionResult> Index(
            [FromQuery] string name,
            [FromQuery] string typeOfProduct,
            [FromQuery] int? minPrice,
            [FromQuery] int? maxPrice,
            [FromQuery] int? productionYear,
            [FromQuery] PartStatus? status,
            [FromQuery] int currentPage = 1,
            [FromQuery] int componentsPerPage = ComponentConstants.ComponentsPerPage)
        {
            try
            {
                var filteredComponents = await _componentService.GetComponentsAsync(
                    name, 
                    typeOfProduct, 
                    minPrice, 
                    maxPrice, 
                    productionYear, 
                    status, 
                    currentPage, 
                    componentsPerPage);

                var topPurchasedComponents = await _componentService.GetTopPurchasedComponentsAsync();

                var response = new
                {
                    FilteredComponents = filteredComponents,
                    TopPurchasedComponents = topPurchasedComponents
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
