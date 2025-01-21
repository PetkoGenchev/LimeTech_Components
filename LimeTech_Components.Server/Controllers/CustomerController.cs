namespace LimeTech_Components.Server.Controllers
{
    using AutoMapper;
    using LimeTech_Components.Server.Services.Components;
    using LimeTech_Components.Server.Services.Customers;
    using Microsoft.AspNetCore.Mvc;

    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpPost("{customerId}/basket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComponentToBasket(string customerId, [FromBody] int componentId)
        {
            try
            {
                var result = await _customerService.AddComponentToBasketAsync(customerId, componentId);

                if (!result)
                {
                    return NotFound("Customer or component not found.");
                }

                return Ok("Component added to basket successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}
