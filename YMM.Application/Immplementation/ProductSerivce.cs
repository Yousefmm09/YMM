using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YMM.Application.Abstract;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
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
                // Validate input
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

                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId,
                    BrandId = dto.BrandId,
                    Variants = dto.Variants?.Select(v => new ProductVariant
                    {
                        Size = v.Size,
                        Color = v.Color,
                        StockQuantity = v.StockQuantity
                    }).ToList() ?? new List<ProductVariant>(),
                    CreatedAt = DateTime.UtcNow // استخدم UTC بدلاً من Now
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

                // تأكد من تحميل العلاقات (Category و Brand)
                // إذا لم يتم تحميلها تلقائياً، استخدم Include في الـ repository

                return new ApiResponse<ProductDto>
                (
                    Success: true,
                    Message: "Product added successfully",
                    Data: new ProductDto
                    {
                        Id = savedProduct.Id, // أضف الـ Id
                        Name = savedProduct.Name,
                        Description = savedProduct.Description,
                        Price = savedProduct.Price,
                        CategoryName = savedProduct.Category?.Name ?? "Unknown", // حماية من null
                        BrandName = savedProduct.Brand?.Name ?? "Unknown", // حماية من null
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
                // Log the exception here
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
            
            var deletedProduct=await  _repo.Delete(productId);
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
            var product=await _repo.GetById(productId);
            if(product == null)
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
        public async Task<UpdateProductDto> UpdateProductDto(UpdateProductDto dto, int productId)
        {
            var existingProduct =  await _repo.UpdateProductDto(dto, productId);
            if(existingProduct ==null) {
                throw new Exception("Product not found");
            }
            return existingProduct;
        }
    }
}
