using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Category;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.ProductDtos;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;

namespace YMM.Application.Abstract.Services
{
    public interface ICategoryService
    {
        Task<string> AddCategory(CategoryDto categoryName);
        Task<ApiResponse<CategoryDetailsDto?>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<string>> DeleteCategory(int id); 
        Task<ApiResponse<CategoryDto>> Update(CategoryDto category);
        Task<ApiResponse<PaginatedResponse<CategoryDetailsDto>>> GetCategoryPagination(PaginationParams pagination);
    }
}
