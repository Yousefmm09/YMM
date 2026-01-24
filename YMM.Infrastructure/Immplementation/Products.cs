using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class Products:IProductRepo
    {
        private readonly AppDb _appDb;
        public Products(AppDb appDb)
        {
            _appDb = appDb;
        }

        public async Task<Product> Add(Product product)
        {
            var p= await _appDb.Products.AddAsync(product);
            await _appDb.SaveChangesAsync();
            return p.Entity;
        }

        public async Task<Product> Delete( int productId)
        {
            var deleteProduct = await _appDb.Products.FindAsync(productId);
            var p=   _appDb.Products.Remove(deleteProduct);
           await  _appDb.SaveChangesAsync();
            return p.Entity; 
        }

        public async Task<List<ProductDto>> GetAll()
        {
            return await _appDb.Products
                .AsNoTracking()
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Variants)
                .Select(x => new ProductDto
                {
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    BrandName = x.Brand.Name,   
                    CategoryName = x.Category.Name,
                    Variants = x.Variants.Select(v => new ProductVariantDto
                    {
                        Size = v.Size,
                        Color = v.Color,
                        StockQuantity = v.StockQuantity
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ProductDto> GetById(int id)
        {
            var product = await _appDb.Products.AsNoTracking().Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Variants)
                .Where(x=> x.Id == id)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    BrandName = x.Brand.Name,
                    CategoryName = x.Category.Name,
                    Variants = x.Variants.Select(v => new ProductVariantDto
                    {
                        Size = v.Size,
                        Color = v.Color,
                        StockQuantity = v.StockQuantity
                    }).ToList()
                }).FirstOrDefaultAsync();
            return product;
        }

        public async Task<ProductDto> GetProductbySKU(string sku)
        {
            var productSku = await _appDb.Products.AsNoTracking().Where(x => x.SKU == sku)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Variants)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    BrandName = x.Brand.Name,
                    CategoryName = x.Category.Name,
                    Variants = x.Variants.Select(x => new ProductVariantDto
                    {
                        Size = x.Size,
                        Color = x.Color,
                        StockQuantity = x.StockQuantity
                    }).ToList(),
                    CreatedAt = x.CreatedAt,
                }).FirstOrDefaultAsync();
            return productSku;
        }

        public async Task<ProductDto> GetProductbySlug(string slug)
        {
            var products =  await _appDb.Products.AsNoTracking().Where(x => x.Slug == slug)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Variants)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    BrandName = x.Brand.Name,
                    CategoryName = x.Category.Name,
                    Slug = x.Slug,
                    Variants = x.Variants.Select(x => new ProductVariantDto
                    {
                        Size = x.Size,
                        Color = x.Color,
                        StockQuantity = x.StockQuantity
                    }).ToList(),
                    CreatedAt = x.CreatedAt
                }).FirstOrDefaultAsync();
            return products;
        }

        public async Task<PaginatedResponse<ProductDto>> GetProductPagination(PaginationParams paginationParams)
        {
            var query = _appDb.Products.AsQueryable();
            var totalItems = query.Count();
            var recordToSkip = (paginationParams.Page - 1) * paginationParams.PageSize;
            var products = await query.AsNoTracking()
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Variants)
                .OrderBy(x => x.Id)
                .Skip(recordToSkip).Take(paginationParams.PageSize)
                .Select(x => new PaginatedResponse<ProductDto>
                {
                    Items = new List<ProductDto>
                    {
                        new ProductDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            Price = x.Price,
                            BrandName = x.Brand.Name,
                            CategoryName = x.Category.Name,
                            CreatedAt = x.CreatedAt,
                            Variants = x.Variants.Select(v => new ProductVariantDto
                            {
                                Size = v.Size,
                                Color = v.Color,
                                StockQuantity = v.StockQuantity
                            }).ToList()
                        }

                    },
                    Page = paginationParams.Page,
                    TotalItems = totalItems,
                    HasNextPage = recordToSkip + paginationParams.PageSize < totalItems,
                    HasPreviousPage = recordToSkip > 0,
                    PageSize = paginationParams.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalItems / paginationParams.PageSize)
                }).ToListAsync();
            return products.FirstOrDefault();

        }

        public async Task<Product> Update(Product product)
        {
            var updatedProduct = _appDb.Products.Update(product);
            await _appDb.SaveChangesAsync();
            return updatedProduct.Entity;
        }
        public async Task<UpdateProductDto> UpdateProductDto(UpdateProductDto dto, int productId)
        {
            var product = await _appDb.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }
            
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.BrandId = dto.BrandId;
            product.CategoryId = dto.CategoryId;
            product.UpdatedAt= DateTime.UtcNow;
            _appDb.Products.Update(product);
            await _appDb.SaveChangesAsync();
            return dto;
        }
        public async Task<List<ProductDto>> GetProductByCategory(string CategoryName)
        {
            var product= await _appDb.Products.AsNoTracking().Include(x=>x.Category).Include(x=>x.Brand)
                .Where(x=>x.Category.Name == CategoryName)
                .Select(x=> new ProductDto
                {
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryName = CategoryName,
                    BrandName=x.Brand.Name,
                    Variants=x.Variants.Select(x=>new ProductVariantDto
                    {
                        Size=x.Size,
                        Color=x.Color,
                        StockQuantity=x.StockQuantity
                    }).ToList(),
                    CreatedAt= DateTime.UtcNow
                }).ToListAsync();
            return product;
        }
    }
}
