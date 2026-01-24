using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;

namespace YMM.Application.Abstract.Services
{
    public interface IBrandService
    {
        Task<string> AddBrand(BrandDto brandName);
    }
}
