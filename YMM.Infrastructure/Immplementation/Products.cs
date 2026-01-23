using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class Products:IProductRepo
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

        public async Task<Product> Delete( int productId)
        {
            var deleteProduct = await _appDb.Products.FindAsync(productId);
            var p=   _appDb.Products.Remove(deleteProduct);
           await  _appDb.SaveChangesAsync();
            return p.Entity; 
        }

        public async Task<List<ProductDto>> GetAll()
        {
            return await _appDb.Products
                .AsNoTracking()
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Variants)
                .Select(x => new ProductDto
                {
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    BrandName = x.Brand.Name,   
                    CategoryName = x.Category.Name,
                    Variants = x.Variants.Select(v => new ProductVariantDto
                    {
                        Size = v.Size,
                        Color = v.Color,
                        StockQuantity = v.StockQuantity
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ProductDto> GetById(int id)
        {
            var product = await _appDb.Products.Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Variants)
                .Where(x=> x.Id == id)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    BrandName = x.Brand.Name,
                    CategoryName = x.Category.Name,
                    Variants = x.Variants.Select(v => new ProductVariantDto
                    {
                        Size = v.Size,
                        Color = v.Color,
                        StockQuantity = v.StockQuantity
                    }).ToList()
                }).FirstOrDefaultAsync();
            return product;
        }

        public async Task<Product> Update(Product product)
        {

            var updatedProduct = _appDb.Products.Update(product);
            await _appDb.SaveChangesAsync();
            return updatedProduct.Entity;
        }
        public async Task<UpdateProductDto> UpdateProductDto(UpdateProductDto dto, int productId)
        {
            var product = await _appDb.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }
            
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.BrandId = dto.BrandId;
            product.CategoryId = dto.CategoryId;
            _appDb.Products.Update(product);
            await _appDb.SaveChangesAsync();
            return dto;
        }
    }
}
