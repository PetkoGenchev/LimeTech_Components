namespace LimeTech_Components.Server.Controllers
{
    using AutoMapper;
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.Services.Components;
    using Microsoft.AspNetCore.Mvc;
    using static LimeTech_Components.Server.Constants.DataConstants;

    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly IComponentService _componentService;
        private readonly IMapper _mapper;

        public HomeController(IComponentService componentService, IMapper mapper)
        {
            _componentService = componentService;
            _mapper = mapper;
        }



        [HttpGet("components")]
        public async Task<IActionResult> Index(
            [FromQuery] string keyword = null,
            [FromQuery] string name = null,
            [FromQuery] string typeOfProduct = null,
            [FromQuery] int? minPrice = null,
            [FromQuery] int? maxPrice = null,
            [FromQuery] int? productionYear = null,
            [FromQuery] PartStatus? status = null,
            [FromQuery] int currentPage = 1,
            [FromQuery] int componentsPerPage = ComponentConstants.ComponentsPerPage)
        {
            try
            {
                var hasFilters = !string.IsNullOrEmpty(keyword) ||
                    !string.IsNullOrEmpty(name) ||
                    !string.IsNullOrEmpty(typeOfProduct) ||
                    minPrice.HasValue ||
                    maxPrice.HasValue ||
                    productionYear.HasValue ||
                    status.HasValue;

                if (hasFilters)
                {

                    var filteredComponents = await _componentService.GetComponentsAsync(
                        keyword,
                        name,
                        typeOfProduct,
                        minPrice,
                        maxPrice,
                        productionYear,
                        status,
                        currentPage,
                        componentsPerPage);

                    return Ok(filteredComponents);
                }
                else
                {
                    var topPurchasedComponents = await _componentService.GetTopPurchasedComponentsAsync();
                    return Ok(topPurchasedComponents);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
