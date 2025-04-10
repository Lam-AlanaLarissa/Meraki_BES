using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTO;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Security.Claims;

namespace Meraki_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly IOrderRepository _orderRepository;

        public PaymentController(IPaymentService paymentService,
            ITransactionService transactionService,
            IAccountService accountService,
            IOrderRepository orderRepository)
        {
            _paymentService = paymentService;
            _transactionService = transactionService;
            _accountService = accountService;
            _orderRepository = orderRepository;
        }

        [HttpGet("create-payment-link")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreatePaymentLink(string orderId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userEmail = identity.Claims.FirstOrDefault().Value;
            var account = await _accountService.GetAccountByEmailAsync(userEmail);
            if (account == null)
            {
                return NotFound();
            }
            var paymentLink = await _paymentService.CreatePaymentLink(orderId, account.AccountId);
            return Ok(paymentLink);
        }

        [HttpGet("get-total-money")]
        public async Task<IActionResult> GetTotalMoneyOfOrder(string orderId)
        {
            var totalMoney = await _orderRepository.GetTotalMoneyOfOrder(orderId);
            return Ok(totalMoney);
        }

        [HttpGet("success-payment")]
        public async Task<IActionResult> SuccessPayment(string transactionId)
        {
            await _paymentService.SuccessPayment(transactionId);
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Payment successful! Your order is on the way to deliver!"
            });
        }
        [HttpGet("failed-payment")]
        public async Task<IActionResult> FailedPayment(string transactionId)
        {
            await _paymentService.FailedPayment(transactionId);
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Payment failed! Please try again!"
            });
        }

    }
}
