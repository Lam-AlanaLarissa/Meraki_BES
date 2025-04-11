using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTO;
using Services.Interfaces;
using System.Security.Claims;

namespace Meraki_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _categoryService;

        public ProductController(IProductService productService, IProductCategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // for Product
        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create-Product")]
        public async Task<IActionResult> CreateProductAsync(string accessToken, [FromBody] CreateProductDTO newProduct)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Must be have access token");
            }
            if (newProduct == null)
            {
                return BadRequest("All field must be filled.");
            }
            var result = await _productService.CreateNewProductAsync(accessToken, newProduct);
            if (result == null)
            {
                return StatusCode(500, new { Message = "Failed to create the Product." });
            }
            return Ok(new
            {
                message = "Create new Product Successfully",
            });
        }


        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("update-Product")]
        public async Task<IActionResult> UpdateProductAsync(string accessToken, string existProductName, [FromForm] UpdateProductDTO updateProduct)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Token must be required");
            }
            if (updateProduct == null)
            {
                return BadRequest("Please fill all fields to update Product");
            }
            var result = await _productService.UpdateProduct(accessToken, existProductName, updateProduct);
            if (result == null)
            {
                return StatusCode(500, new { Message = "Failed to edit the Product." });
            }
            return Ok(new
            {
                Message = "Update Product Successful",
                Result = result
            });
        }

        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("delete-Product")]
        public async Task<IActionResult> DeleteProduct(string accessToken, [FromForm] List<string> ProductIds)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Token must be required");
            }

            if (ProductIds == null && ProductIds.Any())
            {
                return BadRequest("Product Id must be required");
            }

            var result = await _productService.DeleteAProductAsync(accessToken, ProductIds);
            if (result == false)
            {
                return StatusCode(500, new { Message = "Failed to delete the Product." });
            }
            return Ok(new
            {
                Message = "Delete this Product successful",
            });
        }
        // FOR INACTIVE AND ACTIVE Product
        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("inactive-and-active-Product")]
        public async Task<IActionResult> InactiveAndActiveProductBySellerAsync(string accessToken, string ProductId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }
            if (string.IsNullOrEmpty(accessToken) && string.IsNullOrEmpty(ProductId))
            {
                return BadRequest("All field must be required");
            }
            var result = await _productService.InactiveAndActiveProductByOwner(accessToken, ProductId);
            return Ok(result);
        }
        //FOR VIEW

       [HttpGet("Product-detail")]
        public async Task<IActionResult> ViewProductDetailAsync(string ProductId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }
            if (string.IsNullOrEmpty(ProductId))
            {
                return BadRequest("Need productId for getting Product's detail");
            }
            var result = await _productService.ViewProductDetailAsync(ProductId);
            return Ok(result);
        }

        [HttpGet("list-active-products")]
        public async Task<IActionResult> GetListOfProduct([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string sortBy, [FromQuery] bool sortDesc, [FromQuery] string search = null)
        {
            try
            {
                var (Products, totalCount, totalPages) = await _productService.GetListOfActiveProductAsync(pageIndex, pageSize, sortBy, sortDesc, search);
                var response = new
                {
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    Data = Products
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        //[HttpGet("list-all-Products")]
        //public async Task<IActionResult> GetListOfAllProduct([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string sortBy, [FromQuery] bool sortDesc, [FromQuery] string search = null)
        //{
        //    try
        //    {
        //        var (Products, totalCount, totalPages) = await _productService.GetListAllProductAsync(pageIndex, pageSize, sortBy, sortDesc, search);
        //        var response = new
        //        {
        //            TotalCount = totalCount,
        //            PageIndex = pageIndex,
        //            PageSize = pageSize,
        //            TotalPages = totalPages,
        //            Data = Products
        //        };
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
        //    }
        //}

        //[HttpGet("list-Products-of-seller")]
        //public async Task<IActionResult> GetListProductsOfSeller([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string sortBy, [FromQuery] bool sortDesc, [FromQuery] string search = null)
        //{
        //    try
        //    {
        //        var (Products, totalCount, totalPages) = await _productService.GetListProductOfSellerAsync(pageIndex, pageSize, sortBy, sortDesc, search);
        //        var response = new
        //        {
        //            TotalCount = totalCount,
        //            PageIndex = pageIndex,
        //            PageSize = pageSize,
        //            TotalPages = totalPages,
        //            Data = Products
        //        };
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
        //    }
        //}

        //[HttpGet("list-all-Products-of-seller")]
        //public async Task<IActionResult> GetListAllProductsOfSeller([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string sortBy, [FromQuery] bool sortDesc, [FromQuery] string search = null)
        //{
        //    try
        //    {
        //        var (Products, totalCount, totalPages) = await _productService.GetListAllProductOfSellerAsync(pageIndex, pageSize, sortBy, sortDesc, search);
        //        var response = new
        //        {
        //            TotalCount = totalCount,
        //            PageIndex = pageIndex,
        //            PageSize = pageSize,
        //            TotalPages = totalPages,
        //            Data = Products
        //        };
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
        //    }
        //}
        //[HttpGet("list-inactive-Products")]
        //public async Task<IActionResult> GetListInactiveOfProduct([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string sortBy, [FromQuery] bool sortDesc, [FromQuery] string search = null)
        //{
        //    try
        //    {
        //        var (Products, totalCount, totalPages) = await _productService.GetListInactiveProductAsync(pageIndex, pageSize, sortBy, sortDesc, search);
        //        var response = new
        //        {
        //            TotalCount = totalCount,
        //            PageIndex = pageIndex,
        //            PageSize = pageSize,
        //            TotalPages = totalPages,
        //            Data = Products
        //        };
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
        //    }
        //}

        //[HttpGet("list-inactive-Products-of-seller")]
        //public async Task<IActionResult> GetListInactiveProductsOfSeller([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string sortBy, [FromQuery] bool sortDesc, [FromQuery] string search = null)
        //{
        //    try
        //    {
        //        var (Products, totalCount, totalPages) = await _productService.GetListInactiveProductOfSellerAsync(pageIndex, pageSize, sortBy, sortDesc, search);
        //        var response = new
        //        {
        //            TotalCount = totalCount,
        //            PageIndex = pageIndex,
        //            PageSize = pageSize,
        //            TotalPages = totalPages,
        //            Data = Products
        //        };
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
        //    }
        //}

        //[HttpGet("list-Products-categories")]
        //public async Task<IActionResult> GetListCategoryOfProduct()
        //{
        //    var result = await _categoryService.GetListCategoryOfProductAsync();
        //    return Ok(result);
        //}

        //[HttpGet("list-Product-active")]
        //public async Task<IActionResult> GetAllActiveProducts()
        //{
        //    var Products = await _productService.GetAllProductsActive();
        //    return Ok(new ApiResponse()
        //    {
        //        StatusCode = 200,
        //        Data = Products
        //    });
        //}

    }
}
