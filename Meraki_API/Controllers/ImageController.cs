using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using Repositories.DTO;

namespace Meraki_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImageAsync(IFormFile file)
        {
            var result = await _imageService.UploadImageAsync(file, "Meraki");
            if (result == null)
            {
                ModelState.AddModelError("Upload image", " Something went wrong!");
                return Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError);
            }
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Upload new Image successful!",
                Data = new ImageUploadResponse()
                {
                    Link = result.SecureUri.AbsoluteUri,
                    PublicId = result.PublicId
                }
            });
        }
        [HttpGet("generate")]
        public async Task<IActionResult> GenerateAvatar([FromQuery] string fullName)
        {
            try
            {
                string imageUrl = await _imageService.GenerateAndUploadAvatarAsync(fullName);
                return Ok(new { AvatarUrl = imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
