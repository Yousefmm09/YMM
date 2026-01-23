using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;

namespace YMM.Application.Abstract.Services
{
    public interface IProduct
    {
        Task<AddProductDto> Add(AddProductDto dto);
        Task<AddProductDto> Update(AddProductDto dto);
        Task<string> Delete(int productId);
        Task<List<Product>> GetAll();
    }
}
