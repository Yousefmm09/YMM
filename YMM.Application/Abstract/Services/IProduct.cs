using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
