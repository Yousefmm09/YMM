using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Data.Entities;

namespace YMM.Application.Abstract
{
    public interface IProduct
    {
        Task<Product> Add(Product product);
        Task<Product> Update(Product product);
        Task<Product> Delete(Product product);
        Task<List<Product>> GetAll();
    }
}
