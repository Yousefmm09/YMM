using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Brand;
using YMM.Application.Dto.Product;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;

namespace YMM.Application.Immplementation
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepo _brandRepo;
        public BrandService(IBrandRepo brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public async Task<string> AddBrand(CreatBrandDto brandName)
        {
            var BrandDto = new Brand
            {
                Name = brandName.Name,
                Description = brandName.Description,
                Slug = brandName.Slug
            };
            var brand = await _brandRepo.AddAsync(BrandDto);
            if (brand == null)
                return "Failed to add brand";
            return "Brand added successfully";
        }

        public async Task<ApiResponse<string>> DeleteBrand(int brandId)
        {
            var result = await _brandRepo.DeleteBrand(brandId);
            return new ApiResponse<string>
                (
                Success: result != null ? true : false,
                Message: result != null ? "Brand deleted successfully" : "Brand not found",
                Data: result,
                Errors: result == null ? new { BrandId = "Invalid Brand Id" } : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<BrandDto>> GetBrandById(int id)
        {
            var brand = await _brandRepo.GetBrandById(id);
            return new ApiResponse<BrandDto>
             (
                 Success: brand != null ? true : false,
                 Message: brand != null ? "Brand retrieved successfully" : "Brand not found",
                 Data: brand != null ? brand : null,
                 Errors: brand == null ? new { BrandId = "Invalid Brand Id" } : null,
                 TraceId: Guid.NewGuid().ToString()
            );
        }

        public async Task<ApiResponse<BrandFilterDto>> UpdateBrand(BrandFilterDto brand)
        {
            var updatedBrand = await _brandRepo.UpdateAsync(brand);
            return new ApiResponse<BrandFilterDto>
                (
                Success: updatedBrand != null ? true : false,
                Message: updatedBrand != null ? "Brand updated successfully" : "Brand not found",
                Data: updatedBrand,
                Errors: updatedBrand == null ? new { BrandId = "Invalid Brand Id" } : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }
    }

}
