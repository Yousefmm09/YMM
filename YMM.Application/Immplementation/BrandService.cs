using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
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

        public async Task<string> AddBrand(Brand brandName)
        {
            var brand = await _brandRepo.AddAsync(brandName);
            if(brand == null) 
                return "Failed to add brand";
            return "Brand added successfully";
        }
    }
}
