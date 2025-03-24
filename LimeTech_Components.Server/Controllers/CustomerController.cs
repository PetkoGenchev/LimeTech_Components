namespace LimeTech_Components.Server.Controllers
{
    using AutoMapper;
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.DTOs;
    using LimeTech_Components.Server.Services.Components;
    using LimeTech_Components.Server.Services.Customers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("basket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComponentToBasket([FromBody] AddToBasketRequest request)
        {
            var customerId = User.FindFirst("customerId")?.Value; // Extract customer ID from JWT

            if (string.IsNullOrEmpty(customerId))
            {
                Console.WriteLine("Unauthorized: Customer ID missing from token.");
                return Unauthorized();
            }

            try
            {
                var result = await _customerService.AddComponentToBasketAsync(customerId, request.ComponentId);

                if (!result)
                {
                    return NotFound("Customer or component not found.");
                }

                return Ok(new { message = "Component added to basket successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("basket")]
        public async Task<IActionResult> GetBasket()
        {
            var customerId = User.FindFirst("customerId")?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                Console.WriteLine("Unauthorized: Customer ID missing from token.");
                return Unauthorized();
            }

            Console.WriteLine($"Fetching basket for customerId: {customerId}");

            var basket = await _customerService.GetBasketAsync(customerId);

            Console.WriteLine($"Returning basket with {basket.Count} items");
            return Ok(basket);
        }




        [Authorize]
        [HttpDelete("basket/{componentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromBasket(int componentId)
        {
            var customerId = User.FindFirst("customerId")?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                Console.WriteLine("Unauthorized: Customer ID missing from token.");
                return Unauthorized();
            }

            var success = await _customerService.RemoveFromBasketAsync(customerId, componentId);
            if (!success)
            {
                return NotFound("Item not found in basket.");
            }
            return Ok("Item removed successfully.");
        }


        [Authorize]
        [HttpPost("{customerId}/purchase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PurchaseBasket(string customerId)
        {
            var success = await _customerService.PurchaseBasketAsync(customerId);
            if (!success)
            {
                return NotFound("Basket is empty or customer not found.");
            }
            return Ok("Purchase successful.");
        }


        [Authorize]
        [HttpGet("{customerId}/purchase-history")]
        public async Task<IActionResult> GetPurchaseHistory(string customerId)
        {
            var history = await _customerService.GetPurchaseHistoryAsync(customerId);
            return Ok(history);
        }



    }
}
