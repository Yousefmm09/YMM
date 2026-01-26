using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Category;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;

namespace YMM.Application.Immplementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _category;
        public CategoryService(ICategoryRepo categories)
        {
            _category = categories;
        }
        public async Task<string> AddCategory(CategoryDto categoryName)
        {
            var categoryNameExist = new Category
            {
                Name = categoryName.Name,
                Description = categoryName.Description,
                Slug = categoryName.Slug
            };
            var category = await _category.AddAsync(categoryNameExist);
            if(category == null) 
                return "Failed to add category";
            return "Category added successfully";  
        }

        public async Task<ApiResponse<string>> DeleteCategory(int id)
        {
            var category= await _category.DeleteCategory(id);
            return new ApiResponse<string>
                (
                Success: category!= null ? true : false,
                Message: category != null ? "Category deleted successfully" : "Failed to delete category",
                Data: null,
                Errors: category != null ? null : "Category not found",
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<CategoryDetailsDto?>> GetCategoryByIdAsync(int id)
        {
            var category= await _category.GetCategoryByIdAsync(id);
            var categoryDto = new CategoryDetailsDto
            {
                Id=category.Id,
                Name=category.Name,
                Description=category.Description,
                Slug=category.Slug,
                IsActive=category.IsActive,
                ProductCount=category.Products.Count,
                Products =category.Products
            };
            return new ApiResponse<CategoryDetailsDto?>
                (
                Success:category!=null ? true: false,
                Message:category!=null ? "Category retrieved successfully" : "not found Category",
                Data:categoryDto,
                Errors: category != null ? null : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<CategoryDto>> Update(CategoryDto category)
        {
            var updatedCategory = new Category
            {
                Id=category.Id,
                Name = category.Name,
                Description = category.Description,
                Slug = category.Slug,
                IsActive = category.IsActive,
            };
            var categoryDto= new CategoryDto
            {
                Id=updatedCategory.Id,
                Name=updatedCategory.Name,
                Description = updatedCategory.Description,
                Slug = updatedCategory.Slug,
                IsActive = updatedCategory.IsActive,
            };
            var result = await _category.Update(categoryDto);
            return new ApiResponse<CategoryDto>
                (
                Success: result != null ? true : false,
                Message: result != null ? "Category updated successfully" : "Failed to update category",
                Data: categoryDto,
                Errors: result != null ? null : "Category not found",
                TraceId: Guid.NewGuid().ToString()
                );
        }
        public async Task<ApiResponse<PaginatedResponse<CategoryDetailsDto>>> GetCategoryPagination(PaginationParams pagination)
         {
                var categories = await _category.GetCategoryPagination(pagination);
                return new ApiResponse<PaginatedResponse<CategoryDetailsDto>>
                (
                    Success: categories != null ? true : false,
                    Message: categories != null ? "Categories retrieved successfully" : "No categories found",
                    Data: categories,
                    Errors: categories != null ? null :
                    "No categories found",
                    TraceId: Guid.NewGuid().ToString()
                );
        }
    }
}
