using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.ProductDtos;
using YMM.Data.Entities;

namespace YMM.Application.Abstract.Repositories
{
    public interface IBrandRepo
    {
        Task<Brand> AddAsync(Brand brand);
        Task<BrandDto> GetBrandById(int id);
        Task<string> DeleteBrand(int  brandId);
        Task<BrandFilterDto> UpdateAsync(BrandFilterDto brand);

    }
}
