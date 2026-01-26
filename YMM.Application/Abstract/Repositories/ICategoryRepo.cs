using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Category;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;

namespace YMM.Application.Abstract.Repositories
{
    public interface ICategoryRepo
    {
        Task<Category> AddAsync(Category category);
        Task<CategoryDetailsDto?> GetCategoryByIdAsync(int id);
        Task<Category> DeleteCategory(int id);
        Task<CategoryDto> Update(CategoryDto category);
        Task<PaginatedResponse<CategoryDetailsDto>> GetCategoryPagination(PaginationParams pagination);
    }
}
