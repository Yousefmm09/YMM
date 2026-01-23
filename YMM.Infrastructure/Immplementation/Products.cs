using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract;
using YMM.Application.Abstract.Repositories;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class Products:IProduct
    {
        private readonly AppDb _appDb;
        public Products(AppDb appDb)
        {
            _appDb = appDb;
        }

        public async Task<Product> Add(Product product)
        {
            var p= await _appDb.Products.AddAsync(product);
            await _appDb.SaveChangesAsync();
            return p.Entity;
        }

        public async Task<Product> Delete(Product product)
        {
            var p=  _appDb.Products.Remove(product);
           await  _appDb.SaveChangesAsync();
            return p.Entity; 
        }

        public async Task<List<Product>> GetAll()
        {
            var products = await _appDb.Products
                .AsNoTracking().Select(x => new Product
                {
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Brand = new Brand
                    {
                        Name = x.Brand.Name
                    },
                    Category = new Category
                    {
                        Name = x.Category.Name
                    },
                    Variants = x.Variants.Select(v => new ProductVariant
                    {
                        Size = v.Size,
                        Color = v.Color,
                        StockQuantity= v.StockQuantity,
                    }).ToList()
                })
                .ToListAsync();
            return products;
        }

        public async Task<Product> Update(Product product)
        {
            var updatedProduct = _appDb.Products.Update(product);
            await _appDb.SaveChangesAsync();
            return updatedProduct.Entity;
        }
    }
}
