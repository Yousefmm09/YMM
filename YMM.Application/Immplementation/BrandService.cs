using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;

namespace YMM.Application.Immplementation
{
    public class BrandService:IBrandService
    {
        private readonly IBrandRepo _brandRepo;
        public BrandService(IBrandRepo brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public async Task<string> AddBrand(BrandDto brandName)
        {
            var BrandDto= new Brand
            {
                Name = brandName.Name,
                Description = brandName.Description,
                Slug = brandName.Slug
            };
            var brand = await _brandRepo.AddAsync(BrandDto);
            if(brand == null) 
                return "Failed to add brand";
            return "Brand added successfully";
        }
    }
}
