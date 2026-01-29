using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Category;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class CategoryRepo: ICategoryRepo
    {
        private readonly AppDb _appDb;
        public CategoryRepo(AppDb appDb) { _appDb = appDb; }

        public async Task<Category> AddAsync(Category add)
        {
            var category =  await _appDb.Categories.AddAsync(add);
            await _appDb.SaveChangesAsync();
            return category.Entity;
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var getCategory =   _appDb.Categories.Find(id);

            var category = _appDb.Categories.Remove(getCategory);
            await _appDb.SaveChangesAsync();
            return category.Entity;
        }

        public async Task<CategoryDetailsDto?> GetCategoryByIdAsync(int id)
        {
            return await _appDb.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryDetailsDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Slug = c.Slug,
                    IsActive = c.IsActive,

                    ProductCount = _appDb.Products.Count(p => p.CategoryId == c.Id),

                    Products = _appDb.Products
                        .Where(p => p.CategoryId == c.Id)
                        .Select(p => new ProductDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }



        public async Task<CategoryDto> Update(CategoryDto category)
        {
            var getCategory = await _appDb.Categories.FindAsync(category.Id);
            if (getCategory == null)
                return null;
            getCategory.Name=category.Name;
            getCategory.Description=category.Description;
            getCategory.IsActive = category.IsActive;
             _appDb.Categories.Update(getCategory);
            await _appDb.SaveChangesAsync();
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
            };
        }
        public async Task<PaginatedResponse<CategoryDetailsDto>> GetCategoryPagination(PaginationParams pagination)
        {
            // Optimized query: Single database round-trip with proper includes
            // Fixes N+1 query problem identified in performance analysis
            var query = _appDb.Categories
                .AsNoTracking()
                .Include(c => c.Products); // Eager load products to avoid N+1

            var totalItems = await query.CountAsync();

            var categories = await query
                .OrderBy(c => c.Id) // Add explicit ordering for consistent pagination
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(c => new CategoryDetailsDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    ProductCount = c.Products.Count(), // Use loaded collection, not separate query
                    Products = c.Products
                        .Select(p => new ProductDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price
                        }).ToList()
                })
                .ToListAsync();

            return new PaginatedResponse<CategoryDetailsDto>
            {
                Items = categories,
                TotalItems = totalItems,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pagination.PageSize),
                HasNextPage = pagination.Page * pagination.PageSize < totalItems,
                HasPreviousPage = pagination.Page > 1
            };
        }

    }
}
