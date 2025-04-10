using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTO;
using Services.Interfaces;
using System.Security.Claims;

namespace Meraki_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrderFromCartItemSelected([FromForm] CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }
            if (orderDTO == null)
            {
                return BadRequest("All fields must be filled in");
            }
            var result = await _orderService.CreateAnOrderFromCartAsync(orderDTO);

            return Ok(result);
        }

        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("checkout-order")]
        public async Task<IActionResult> CreateOrderForCheckout(CheckoutRequest request)
        {
            await _orderService.CheckoutRequest(request);
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Checkout successful! Please call API Payment for the next steps!"
            });
        }

        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("finish-delivering-stage")]
        public async Task<IActionResult> FinishDeliveringStage(string orderId)
        {
            await _orderService.FinishDeliveringStage(orderId);
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Finish delivering stage!"
            });
        }



        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Data = orders
            });
        }







    }
}
