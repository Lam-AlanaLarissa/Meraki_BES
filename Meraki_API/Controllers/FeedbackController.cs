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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [Authorize(Policy = "UserOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("add-feedback")]
        public async Task<IActionResult> CreateFeedbackAsync(CreateFeedbackDTO feedbackDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userEmail = identity.Claims.FirstOrDefault().Value;
            if (feedbackDTO == null)
            {
                return BadRequest("All fields are required");
            }
            var result = await _feedbackService.CreateFeedbackAsync(userEmail, feedbackDTO);
            return Ok(result);
        }

        [HttpGet("list-feedbacks-of-product")]
        public async Task<IActionResult> GetListFeedbackOfproductAsync([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string sortBy, [FromQuery] bool sortDesc, [FromQuery] string search = null)
        {
            try
            {
                var (feedbacks, totalCount, totalPages) = await _feedbackService.GetListFeedbackOfProduct(pageIndex, pageSize, sortBy, sortDesc, search);
                var response = new
                {
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    Data = feedbacks
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

    }
}
