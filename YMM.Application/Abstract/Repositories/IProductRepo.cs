using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Product;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;

namespace YMM.Application.Abstract
{
    public interface IProductRepo
    {
        Task<Product> Add(Product product);
        Task<Product> Update(Product product);
        Task<Product> Delete(int id);
        Task<List<ProductDto>> GetAll();
        Task<ProductDto> GetById(int id);
        Task<UpdateProductDto> UpdateProductDto(UpdateProductDto dto, int productId);

        Task<PaginatedResponse<ProductDetailDto>> GetProductPagination(PaginationParams paginationParams);
        Task<ProductDto> GetProductbySlug(string slug);
        Task<ProductDto> GetProductbySKU(string sku);
        Task<List<ProductDto>> GetProductByCategory(string CategoryName);
        Task<List<ProductFiltersResponseDto>> GetProductFilterByFilter(PaginationParams pagination, ProductFilterDto dto);
        Task<ApiResponse<List<ProductSearchSuggestionDto>>> GetProductSearchSuggestions(string query);
    }
}
