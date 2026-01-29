using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YMM.Application.Abstract;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Product;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;

namespace YMM.Application.Immplementation
{
    public class ProductSerivce : IProduct
    {
        private readonly IProductRepo _repo;
        public ProductSerivce(IProductRepo productRepo)
        {
            _repo = productRepo;
        }
        public async Task<ApiResponse<ProductDto>> Add(AddProductDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ApiResponse<ProductDto>
                    (
                        Success: false,
                        Message: "Invalid product data",
                        Data: null,
                        Errors: new[] { "Product data is required" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }
                var slug = Helper.SlugHelper.GenerateSlug(dto.Name);
                var Sku = $"{slug}-{DateTime.Now.Ticks}";
                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId,
                    BrandId = dto.BrandId,
                    Slug = slug,
                    SKU = Sku,
                    Variants = dto.Variants?.Select(v => new ProductVariant
                    {
                        Size = v.Size,
                        Color = v.Color,
                        StockQuantity = v.StockQuantity
                    }).ToList() ?? new List<ProductVariant>(),
                    IsNew = true,
                    IsActive = true,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow
                };

                var savedProduct = await _repo.Add(product);

                if (savedProduct == null)
                {
                    return new ApiResponse<ProductDto>
                    (
                        Success: false,
                        Message: "Error adding product",
                        Data: null,
                        Errors: new[] { "Failed to save product to database" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

                return new ApiResponse<ProductDto>
                (
                    Success: true,
                    Message: "Product added successfully",
                    Data: new ProductDto
                    {
                        Name = savedProduct.Name,
                        Description = savedProduct.Description,
                        Price = savedProduct.Price,
                        CategoryName = savedProduct.Category?.Name ?? "Unknown",
                        BrandName = savedProduct.Brand?.Name ?? "Unknown",
                        Slug = savedProduct.Slug,
                        SKU = savedProduct.SKU,
                        Variants = savedProduct.Variants?.Select(v => new ProductVariantDto
                        {
                            Size = v.Size,
                            Color = v.Color,
                            StockQuantity = v.StockQuantity
                        }).ToList() ?? new List<ProductVariantDto>(),
                        CreatedAt = savedProduct.CreatedAt
                    },
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                );
            }
            catch (Exception ex)
            {

                return new ApiResponse<ProductDto>
                (
                    Success: false,
                    Message: "An error occurred while adding the product",
                    Data: null,
                    Errors: new[] { ex.Message },
                    TraceId: Guid.NewGuid().ToString()
                );
            }
        }
        public async Task<string> Delete(int productId)
        {

            var deletedProduct = await _repo.Delete(productId);
            if (deletedProduct == null)
                return "Error deleting product";
            return $"Delete the Product has name {deletedProduct.Name} success";
        }

        public async Task<List<ProductDto>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<ProductDto> GetById(int productId)
        {
            var product = await _repo.GetById(productId);
            if (product == null)
                throw new Exception("Product not found");
            return new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.CategoryName,
                BrandName = product.BrandName,
                Variants = product.Variants.Select(v => new ProductVariantDto
                {
                    Size = v.Size,
                    Color = v.Color,
                    StockQuantity = v.StockQuantity
                }).ToList()
            };

        }

        public async Task<ApiResponse<ProductDto>> GetProductbySKU(string sku)
        {
            //var text = Helper.SlugHelper.GenerateSlug(te);
            //var sku = $"{}-{DateTime.Now.Ticks}";
            var product = await _repo.GetProductbySKU(sku);
            if (product is null)
                if (product == null)
                {
                    return new ApiResponse<ProductDto>
                    (
                        Success: false,
                        Message: "Product not found",
                        Data: null,
                        Errors: new[] { $"Product with sku '{sku}' does not exist" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }
            return new ApiResponse<ProductDto>
            (
              Success: true,
              Message: "Product retrieved successfully",
              Data: product,
              Errors: null,
               TraceId: Guid.NewGuid().ToString()
             );
        }

        public async Task<ApiResponse<ProductDto>> GetProductbySlug(string slug)
        {
            //var text =Helper.SlugHelper.GenerateSlug(slug);
            var product = await _repo.GetProductbySlug(slug);
            if (product is null)
                if (product == null)
                {
                    return new ApiResponse<ProductDto>
                    (
                        Success: false,
                        Message: "Product not found",
                        Data: null,
                        Errors: new[] { $"Product with slug '{slug}' does not exist" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }
            return new ApiResponse<ProductDto>
            (
              Success: true,
              Message: "Product retrieved successfully",
              Data: product,
              Errors: null,
               TraceId: Guid.NewGuid().ToString()
             );
        }

        public async Task<ApiResponse<PaginatedResponse<ProductDetailDto>>> GetProductPagination(PaginationParams paginationParams)
        {
            var prod = await _repo.GetProductPagination(paginationParams);
            return new ApiResponse<PaginatedResponse<ProductDetailDto>>
                (
                Success: prod != null ? true : false,
                Message: prod != null ? "The Products is Retrive Successs" : "Not Found Products",
                Data: prod != null ? prod : null,
                Errors: prod != null ? null : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<UpdateProductDto> UpdateProductDto(UpdateProductDto dto, int productId)
        {
            var existingProduct = await _repo.UpdateProductDto(dto, productId);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }
            return existingProduct;
        }
        public async Task<ApiResponse<List<ProductDto>>> GetProductByCategory(string CategoryName)
        {
            var product = await _repo.GetProductByCategory(CategoryName);
            return new ApiResponse<List<ProductDto>>
                 (
                 Success: product != null ? true : false,
                 Message: product != null ? $"The Category {CategoryName} have is Retrive Successs, The Count of Product is = {product.Count()}" : $"The Category has name {CategoryName} not have product now",
                 Data: product != null ? product : null,
                 Errors: product != null ? null : null,
                 TraceId: Guid.NewGuid().ToString()
                 );
        }
    }
}
