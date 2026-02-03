using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class BrandRepo : IBrandRepo
    {
        private readonly AppDb _appDb;
        public BrandRepo(AppDb appDb)
        {
            _appDb = appDb;
        }
        public async Task<Brand> AddAsync(Brand add)
        {
            var category = await _appDb.Brands.AddAsync(add);
            await _appDb.SaveChangesAsync();
            return category.Entity;
        }

        public async Task<string> DeleteBrand(int brandId)
        {
            var Getbrand = await _appDb.Brands.FindAsync(brandId);
            if (Getbrand == null)
                return null;
            var brand= _appDb.Brands.Remove(Getbrand);
            await _appDb.SaveChangesAsync();
            return "the Brand Remove Success";
        }

        public async Task<BrandDto> GetBrandById(int id)
        {
            var brand =  await _appDb.Brands
                .AsNoTracking().Where(x => x.Id == id)
                .Select(x => new BrandDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Slug = x.Slug,
                    ProductCount = _appDb.Products.Where(x => x.BrandId == id).Count()
                }).FirstOrDefaultAsync();
            return brand;
        }

        public async Task<BrandFilterDto> UpdateAsync(BrandFilterDto brand)
        {
            var Getbrand = await _appDb.Brands.FindAsync(brand.Id);
            if(Getbrand == null)
                return null;
            Getbrand.Name = brand.Name;
            _appDb.Brands.Update(Getbrand);
            await _appDb.SaveChangesAsync();
            return brand;
        }
    }
}
