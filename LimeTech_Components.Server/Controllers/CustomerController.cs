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
                return Unauthorized();
            }

            var basket = await _customerService.GetBasketAsync(customerId);

            return Ok(basket);
        }




        [Authorize]
        [HttpDelete("basket")]
        public async Task<IActionResult> RemoveFromBasket([FromBody] List<int> componentIds)
        {
            var customerId = User.FindFirst("customerId")?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                return Unauthorized();
            }

            bool success = await _customerService.RemoveFromBasketAsync(customerId, componentIds);

            if (!success)
            {
                return NotFound(new { message = "No matching items found in the basket." });
            }

            return Ok(new { message = "Selected items removed from the basket.", success = true });
        }


        [Authorize]
        [HttpPost("purchases")]
        public async Task<IActionResult> PurchaseBasket([FromBody] List<int> componentIds)
        {
            var customerId = User.FindFirst("customerId")?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                return Unauthorized();
            }

            var (success, purchasedItems, totalCost) = await _customerService.PurchaseSelectedItemsAsync(customerId, componentIds);

            if (!success)
            {
                return NotFound("No valid items selected for purchase.");
            }

            return Ok(new
            {
                Message = $"Purchased {purchasedItems.Count} items for a total of {totalCost:F2} BGN",
                PurchasedItems = purchasedItems
            });
        }



        [Authorize]
        [HttpGet("purchases")]
        public async Task<IActionResult> GetPurchaseHistory()
        {
            var customerId = User.FindFirst("customerId")?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                return Unauthorized();
            }

            var history = await _customerService.GetPurchaseHistoryAsync(customerId);

            return Ok(history);
        }
    }
}
