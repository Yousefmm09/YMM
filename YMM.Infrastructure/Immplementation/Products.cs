using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Product;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<PaginatedResponse<ProductDetailDto>> GetProductPagination(PaginationParams paginationParams)
        {
            
            var query = _appDb.Products
                .AsNoTracking()
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Variants);

            var totalItems = await query.CountAsync();
            var recordToSkip = (paginationParams.Page - 1) * paginationParams.PageSize;

            var products = await query
                .OrderBy(x => x.Id)
                .Skip(recordToSkip)
                .Take(paginationParams.PageSize)
                .Select(x => new ProductDetailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    BrandName = x.Brand.Name,
                    CategoryName = x.Category.Name,
                    CreatedAt = x.CreatedAt,
                    SKU = x.SKU,
                    Slug = x.Slug,
                    AvailableSizes=new List<string>(x.Variants.Select(v=>v.Size).Distinct()),
                    AvailableColors=new List<string>(x.Variants.Select(v=>v.Color).Distinct()),
                })
                .ToListAsync();

            return new PaginatedResponse<ProductDetailDto>
            {
                Items = products,
                Page = paginationParams.Page,
                TotalItems = totalItems,
                HasNextPage = recordToSkip + paginationParams.PageSize < totalItems,
                HasPreviousPage = recordToSkip > 0,
                PageSize = paginationParams.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / paginationParams.PageSize)
            };
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
        public async Task<List<ProductFiltersResponseDto>> GetProductFilterByFilter(PaginationParams pagination,ProductFilterDto dto)
        {
            var query = _appDb.Products.AsNoTracking()
                .Include(x => x.Category).Include(x=>x.Variants)
                .Include(x => x.Brand).AsQueryable();
            var queryVariant=_appDb.Products.Include(x=>x.Variants).AsNoTracking();
            var totalCount = await query.CountAsync();
            var SkipParam = (pagination.Page - 1) * pagination.PageSize;
            // Applay Filter
            // Filter by Search
            if (!string.IsNullOrEmpty(dto.Search))
                query = query.Where(x => x.Name.Contains(dto.Search) || x.Description.Contains(dto.Search));

                if (dto.CategoryId > 0)
                    query = query.Where(x => x.CategoryId == dto.CategoryId);
                if(dto.MinPrice.HasValue && dto.MaxPrice.HasValue)
                    query=query.Where(x=>x.Price>dto.MinPrice &&x.Price< dto.MaxPrice);
            query = dto.SortBy switch
            {
                "Price_Desc" => query.OrderByDescending(x => x.Price),
                "Price_Asc" => query.OrderBy(x => x.Price),
                "Newest" => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderBy(x => x.Id)
            };
            query = dto.Color switch
            {
                "Red"=>query.Where(x=>x.Variants.Any(x=>x.Color==dto.Color)),
                "Blue"=>query.Where(x=>x.Variants.Any(x=>x.Color==dto.Color)),
                "Green"=>query.Where(x=>x.Variants.Any(x=>x.Color==dto.Color)),
                _ => query
            };
            query = dto.Size switch
            {
                "32" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "33" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "34" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "35" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "36" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "37" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "38" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "39" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "40" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "41" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "42" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "43" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                "44" => query.Where(x => x.Variants.Any(x => x.Size == dto.Size)),
                _=>query
            };
            return await query.Skip(SkipParam).Take(pagination.PageSize)
                   .Select(x => new ProductFiltersResponseDto
                   {

                       ProductName=x.Name,
                       Sizes = x.Variants.Select(v => v.Size).Distinct().ToList(),
                       Colors = x.Variants.Select(v => v.Color).Distinct().ToList(),
                       SingleColor=dto.Color,
                       MaxPrice = x.Price,
                       Brands = new List<BrandFilterDto>        
                        {
                            new BrandFilterDto
                            {
                                Id = x.Brand.Id,
                                Name = x.Brand.Name
                            }
                        },
                       Categories = new List<CategoryFilterDto>
                        {
                            new CategoryFilterDto
                            {
                                Id = x.Category.Id,
                                Name = x.Category.Name
                            }
                        }
                   }).ToListAsync();

        }
     public async Task<ApiResponse<List<ProductSearchSuggestionDto>>> GetProductSearchSuggestions(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
             {
                return new ApiResponse<List<ProductSearchSuggestionDto>>
              (
                  Success: false,
                    Message: "Search query is required",
                    Data: null,
                    Errors: new[] { "Query cannot be empty" },
                    TraceId: Guid.NewGuid().ToString()
              );
            }
            if (query.Length <= 2)
                return new ApiResponse<List<ProductSearchSuggestionDto>>
                    (
                     Success: false,
                     Message: "The query is short",
                     Data: null,
                     Errors: new[] { "Please enter at least 2 characters" },
                     TraceId: Guid.NewGuid().ToString()
                    );
            var ProductQuery =  _appDb.Products.Include(x => x.Variants).Include(x => x.Category)
                .Include(x => x.Brand).Where(x => !x.IsDeleted).AsQueryable();

            var Search = ProductQuery.Where(x => EF.Functions.Like(x.Name, $"%{query}%"))
                .OrderByDescending(x => x.SoldCount)
                .Take(10);

            if (Search ==null)
            {
                return new ApiResponse<List<ProductSearchSuggestionDto>>
                   (
                    Success: false,
                    Message: "Not found product",
                    Data: null,
                    Errors: new[] { "your search not matched in any product" },
                    TraceId: Guid.NewGuid().ToString()
                   );
            }
            var RepsonseSearch = await Search.Select(x => new ProductSearchSuggestionDto
            {
                Id=x.Id,
                Name=x.Name,
                BrandName=x.Brand.Name,
                Slug=x.Slug,
                Price=x.Price,
                CategoryName=x.Category.Name,
            }).ToListAsync();
            return new ApiResponse<List<ProductSearchSuggestionDto>>
           (
               Success: true,
               Message: "Suggestions retrieved successfully",
               Data: RepsonseSearch,
               Errors: null,
               TraceId: Guid.NewGuid().ToString()
           );
        }
    }
}
