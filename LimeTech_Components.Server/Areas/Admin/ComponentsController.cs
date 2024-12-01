namespace LimeTech_Components.Server.Areas.Admin
{
    using AutoMapper;
    using LimeTech_Components.Server.DTOs;
    using LimeTech_Components.Server.Services.Components;
    using LimeTech_Components.Server.Services.Components.Models;
    using Microsoft.AspNetCore.Mvc;
    using static LimeTech_Components.Server.Constants.DataConstants;

    [ApiController]
    [Route("api/components")]
    public class ComponentsController : AdminController
    {
        private readonly IComponentService _componentService;
        private readonly IMapper _mapper;

        public ComponentsController(IComponentService componentService, IMapper mapper)
        {
            _componentService = componentService;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComponents(
        [FromQuery] int currentPage = 1,
        [FromQuery] int componentsPerPage = ComponentConstants.ComponentsPerPage)
        {
            // Validate input parameters if necessary
            if (currentPage < 1 || componentsPerPage < 1)
            {
                return BadRequest("Current page and components per page must be greater than zero.");
            }

            try
            {
                var components = await _componentService.GetComponentsAsync(
                    currentPage: currentPage,
                    componentsPerPage: componentsPerPage,
                    publicOnly: false);

                return Ok(components);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, "An error occurred while retrieving components.");
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComponent([FromBody] AddComponentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var componentServiceModel = new ComponentServiceModel
                {

                }

                await _componentService.AddComponentAsync(componentServiceModel);

                return StatusCode(201, "Component added successfully."); // 201 Created
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }






    }
}
