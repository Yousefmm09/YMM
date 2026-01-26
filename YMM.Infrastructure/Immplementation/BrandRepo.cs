using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
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
    }
}
