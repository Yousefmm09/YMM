using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Product;
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
    }
}
