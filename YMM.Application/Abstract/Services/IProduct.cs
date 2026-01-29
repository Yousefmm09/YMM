using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Product;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;

namespace YMM.Application.Abstract.Services
{
    public interface IProduct
    {
        Task<ApiResponse<ProductDto>> Add(AddProductDto dto);
        Task<string> Delete(int productId);
        Task<List<ProductDto>> GetAll();
        Task<ProductDto> GetById(int productId);

        Task<UpdateProductDto> UpdateProductDto(UpdateProductDto dto, int productId);
        Task<ApiResponse<PaginatedResponse<ProductDetailDto>>> GetProductPagination(PaginationParams paginationParams);
        Task<ApiResponse<ProductDto>> GetProductbySlug(string slug);
        Task<ApiResponse<ProductDto>> GetProductbySKU(string sku);
        Task<ApiResponse<List<ProductDto>>> GetProductByCategory(string CategoryName);


    }
}
