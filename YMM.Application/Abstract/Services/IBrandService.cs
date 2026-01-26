using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Brand;
using YMM.Application.Dto.Product;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;

namespace YMM.Application.Abstract.Services
{
    public interface IBrandService
    {
        Task<string> AddBrand(CreatBrandDto brandName);
        Task<ApiResponse<BrandDto>> GetBrandById(int id);
        Task<ApiResponse<string>> DeleteBrand(int brandId);
        Task<ApiResponse<BrandFilterDto>> UpdateBrand(BrandFilterDto brand);
    }
}
