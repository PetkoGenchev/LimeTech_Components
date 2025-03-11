namespace LimeTech_Components.Server.Controllers
{
    using AutoMapper;
    using LimeTech_Components.Server.DTOs;
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
        public async Task<IActionResult> AddComponentToBasket(string customerId, [FromBody] AddToBasketRequest request)
        {
            try
            {
                var result = await _customerService.AddComponentToBasketAsync(customerId, request.ComponentId);

                if (!result)
                {
                    return NotFound("Customer or component not found.");
                }

                return Ok("Component added to basket successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [HttpGet("{customerId}/basket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBasket(string customerId)
        {
            var basket = await _customerService.GetBasketAsync(customerId);
            if (basket == null)
            {
                return NotFound("Basket not found.");
            }
            return Ok(basket);
        }



        [HttpDelete("{customerId}/basket/{componentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromBasket(string customerId, int componentId)
        {
            var success = await _customerService.RemoveFromBasketAsync(customerId, componentId);
            if (!success)
            {
                return NotFound("Item not found in basket.");
            }
            return Ok("Item removed successfully.");
        }



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



        [HttpGet("{customerId}/purchase-history")]
        public async Task<IActionResult> GetPurchaseHistory(string customerId)
        {
            var history = await _customerService.GetPurchaseHistoryAsync(customerId);
            return Ok(history);
        }



    }
}
