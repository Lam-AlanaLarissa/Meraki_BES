using CloudinaryDotNet.Actions;
using Repositories.DTO;
using Repositories.Implements;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _product;
        private readonly IAccountRepository _account;
        private readonly IProductCategoryRepository _category;
        private readonly IImageService _image;

        public ProductService(IProductRepository product, IAccountRepository account, IProductCategoryRepository category, IImageService image)
        {
            _product = product;
            _account = account;
            _category = category;
            _image = image;
        }

        public async Task<string> AutoGenerateProductId()
        {
            string newProductID = "";
            string latestUserId = await _product.GetLatestProductIdAsync();
            if (string.IsNullOrEmpty(latestUserId))
            {
                newProductID = "P000000001";
            }
            else
            {
                int numericpart = int.Parse(latestUserId.Substring(1));
                int newnumericpart = numericpart + 1;
                newProductID = $"P{newnumericpart:d9}";
            }
            return newProductID;
        }

        // FOR CRUD Product
        public async Task<dynamic> CreateNewProductAsync(string accessToken, CreateProductDTO newProduct)
        {
            try
            {
                var accEmail = TokenDecoder.GetEmailFromToken(accessToken);
                var acc = await _account.GetAccountByEmailAsync(accEmail);
                if (newProduct.ProductPrice == 0)
                {
                    newProduct.ProductPrice = newProduct.ProductPrice;
                }


                var attachmentUris = new List<string>();
                //Upload each attachment file
                if (newProduct.Attachments != null && newProduct.Attachments.Any())
                {
                    // Upload each file and collect its URI
                    foreach (var file in newProduct.Attachments)
                    {
                        var attachment = await _image.UploadImageAsync(file, "product");
                        attachmentUris.Add(attachment.SecureUri.AbsoluteUri);
                    }
                }
                var product = new Product
                {
                    ProductId = await AutoGenerateProductId(),
                    ProductName = newProduct.ProductName,
                    PcateId = newProduct.PcateId,
                    ProductDesc = newProduct.ProductDesc,
                    Discount = newProduct.Discount,// tổng kích thước hoa có sẵn
                    Location = newProduct.Location,// tổng số lượng hoa có 
                    PurchaseType = newProduct.PurchaseType,
                    PurchaseDate = newProduct.PurchaseDate,
                    PercentageOfDamage = newProduct.PercentageOfDamage,
                    AccountId = acc.AccountId,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    CreatedBy = acc.AccountId,
                    Status = newProduct.Status,
                    Images = string.Join(",", attachmentUris),//thêm blob storage sau
                };

                var result = await _product.CreateProductAsync(product);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at CreateNewProductAsync() + {ex.Message}");
            }
        }

        public async Task<dynamic> UpdateProduct(string accessToken, UpdateProductDTO updateProduct)
        {
            try
            {
                // Determine the acc
                string ownerEmail = TokenDecoder.GetEmailFromToken(accessToken);
                var accOwner = await _account.GetAccountByEmailAsync(ownerEmail);
                if (accOwner == null)
                {
                    return "Account is not found or invalid token";
                }
                // Check the exist of product
                var product = await _product.GetProductByProductNameAsync(updateProduct.ProductName);
                if (product == null)
                {
                    return "Product is not found";
                }
                // Determine the acc's ownership of Products
                if (accOwner.AccountId != product.AccountId)
                {
                    return "You have no permission to update this product";
                }

                // Validation all field of update product and update product
                if (string.IsNullOrEmpty(updateProduct.ProductName))
                {
                    updateProduct.ProductName = product.ProductName;
                }
                product.ProductName = updateProduct.ProductName;

                if (string.IsNullOrEmpty(updateProduct.PcateId))
                {
                    updateProduct.PcateId = product.PcateId;
                }
                product.PcateId = updateProduct.PcateId;

                if (string.IsNullOrEmpty(updateProduct.ProductDesc))
                {
                    updateProduct.ProductDesc = product.ProductDesc;
                }
                product.ProductDesc = updateProduct.ProductDesc;

                if (string.IsNullOrEmpty(updateProduct.PurchaseType))
                {
                    updateProduct.PurchaseType = product.PurchaseType;
                }
                product.PurchaseType = updateProduct.PurchaseType;

                if (string.IsNullOrEmpty(updateProduct.DamageDetail))
                {
                    updateProduct.DamageDetail = product.DamageDetail;
                }
                product.DamageDetail = updateProduct.DamageDetail;

                if (updateProduct.ProductPrice == 0)
                {
                    updateProduct.ProductPrice = product.ProductPrice;
                }
                product.ProductPrice = updateProduct.ProductPrice;



                product.Discount = updateProduct.Discount;

                var attachmentUris = new List<string>();
                //Upload each attachment file
                if (updateProduct.Attachments == null && updateProduct.Attachments.Any())
                {
                    attachmentUris.Add(product.Images);
                }
                foreach (var file in updateProduct.Attachments)
                {
                    var attachment = await _image.UploadImageAsync(file, "product");
                    attachmentUris.Add(attachment.SecureUri.AbsoluteUri);
                }
                product.Images = string.Join(",", attachmentUris);

                if (updateProduct.PurchaseDate == DateOnly.MinValue || updateProduct.PurchaseDate == DateOnly.MaxValue)
                {
                    updateProduct.PurchaseDate = product.PurchaseDate;
                }
                product.PurchaseDate = updateProduct.PurchaseDate;

                product.UpdatedBy = accOwner.AccountId;
                product.UpdatedAt = DateTime.Now;
                var result = await _product.UpdateProductAsync(product);
                return new
                {
                    result,
                    product
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at UpdateAProductAsync() in Service: {ex.Message}");
            }
        }

        public async Task<dynamic> DeleteAProductAsync(string accessToken, List<string> productIds)
        {
            bool isSuccess = true;

            string ownerEmail = TokenDecoder.GetEmailFromToken(accessToken);
            var owner = await _account.GetAccountByEmailAsync(ownerEmail);
            foreach (var productId in productIds)
            {
                var product = await _product.GetProductByProductIdAsync(productId);

                if (product == null)
                {

                    isSuccess = false;
                    continue; // Move to the next product ID
                }

                if (product.AccountId != owner.AccountId)
                {
                    isSuccess = false;
                    continue; // Move to the next product ID
                }

                product.Status = "IsDeleted";
                var result = await _product.UpdateProductAsync(product);

                if (!result)
                {
                    isSuccess = false;
                }
            }

            return isSuccess;
        }

        public async Task<dynamic> InactiveAndActiveProductByOwner(string accessToken, string productId)
        {
            var accEmail = TokenDecoder.GetEmailFromToken(accessToken);
            var acc = await _account.GetAccountByEmailAsync(accEmail);
            if (acc == null)
            {
                return new
                {
                    Message = "Cannot find this account",
                    Status = 404
                };
            }

            var product = await _product.GetProductByProductIdAsync(productId);
            if (product == null)
            {
                return new
                {
                    Message = "Product cannot be found",
                    StatusCode = 404,
                };
            }
            if (product.Status == "Active")
            {
                product.Status = "Inactiove";
                await _product.UpdateProductAsync(product);
                return new
                {
                    Message = "Inactive Successful",
                };
            }
            else if (product.Status == "Inactive")
            {
                product.Status = "Active";
                await _product.UpdateProductAsync(product);
                return new
                {
                    Message = "Active Successful",
                };

            }
            return new
            {
                Message = "Failure"
            };

        }

        // FOR VIEW PRODUCT
        public async Task<dynamic> ViewProductDetailAsync(string productId)
        {
            var product = await _product.GetProductByProductIdAsync(productId);
            if (product == null)
            {
                return new
                {
                    Message = "Cannot find this product",
                    StatusCode = 404
                };
            }
            var ProductCate = await _category.GetProductCategoryByCateIdAsync(product.PcateId);
            var acc = await _account.GetAccountById(product.AccountId);
            var ProductInfo = new DetailProductDTO
            {
                ProductId = productId,
                ProductName = product.ProductName,
                PCateName = ProductCate.PcateName,
                CustomerName = acc.FullName,
                ProductDesc = product.ProductDesc,
                Discount = product.Discount,
                Location = product.Location,
                ProductPrice = product.ProductPrice,
                PurchaseDate = product.PurchaseDate,
                PurchaseType = product.PurchaseType,
                PercentageOfDamage = product.PercentageOfDamage,
                DamageDetail = product.DamageDetail,
                Status = product.Status,
                Images = product.Images,
                CreatedAt = product.CreatedAt,
                CreatedBy = product.CreatedBy,
                UpdatedAt = product.UpdatedAt,
                UpdatedBy = product.UpdatedBy,
                DeletedAt = product.DeletedAt,
                DeletedBy = product.DeletedBy,
            };
            return ProductInfo;
        }

        public async Task<(List<ProductListDTO> products, int totalCount, int totalPages)> GetListOfActiveProductAsync(int pageIndex, int pageSize, string sortBy, bool sortDesc, string search)
        {
            return await _product.GetListOfActiveProduct(pageIndex, pageSize, sortBy, sortDesc, search);
        }
    }
}
