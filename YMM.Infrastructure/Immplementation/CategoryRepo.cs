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
    public class CategoryRepo: ICategoryRepo
    {
        private readonly AppDb _appDb;
        public CategoryRepo(AppDb appDb) { _appDb = appDb; }

        public async Task<Category> AddAsync(Category add)
        {
            var category =  await _appDb.Categories.AddAsync(add);
            await _appDb.SaveChangesAsync();
            return category.Entity;
        }
    }
}
