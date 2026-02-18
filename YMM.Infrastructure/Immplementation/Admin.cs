using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Admin;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Order;
using YMM.Application.Dto.ProductDtos;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class Admin : IAdminRepo
    {
        private readonly IDbContextFactory<AppDb> _appDb;
        private readonly AppDb db1;
        public Admin(IDbContextFactory<AppDb> db,AppDb app)
        {
         _appDb = db;   
            db1 = app;
        }
        public async Task<ApiResponse<DashboardStatsDto>> DashboardStats()
        {
            var TotalProduct = await TotalProducts();
            var Customer = await CustomerStats();
            var ActiveProducts = await ActiveProduct();
            var LowProducts = await LowStockProducts();
            var OutOfStockProduct = await OutOfStockProducts();
            var CountCustomer = await CountCustomers();
           //await Task.WhenAll(TotalProducts(), CustomerStats(),ActiveProduct(),LowStockProducts(),OutOfStockProducts()
           //    ,CountCustomers());
            return new ApiResponse<DashboardStatsDto>
                (
                Success:true,
                Message:"The Dashboard Stats Retrived success",
                Data: new DashboardStatsDto
                {
                    TotalProducts = TotalProduct,
                    ActiveProducts= ActiveProducts,
                    LowStockProducts= LowProducts,
                    OutOfStockProducts=OutOfStockProduct,
                    RecentCustomers =Customer,
                    TotalCustomers=CountCustomer
                },
               Errors: null,
               TraceId:Guid.NewGuid().ToString()
                );
        }
        
        private async Task<int> TotalProducts()
        {
            using var context = _appDb.CreateDbContext();
            var product = await context.Products.AsNoTracking().CountAsync();
            return product;
       }
           
        private async Task<int> ActiveProduct()
        {
            using var context = _appDb.CreateDbContext();
            var ActiveProducts = await context.Products.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return ActiveProducts;

        }
        private async Task<List<LowStockProduct>> LowStockProducts()
        {
            using var context = _appDb.CreateDbContext();

            var products = context.ProductVariants.AsNoTracking().Where(x => x.StockQuantity < 10).Select(x => new LowStockProduct
            {
                ProductName = x.Product.Name,
                QuantityStock = x.StockQuantity,
            }).ToList();
            return products;
        }
        private async Task<List<OutofStock>> OutOfStockProducts()
        {
            using var context = _appDb.CreateDbContext();

            var products = context.ProductVariants.AsNoTracking().Where(x => x.StockQuantity == 0).Select(x => new OutofStock
            {
                ProductName = x.Product.Name,
            }).ToList();
            return products;
        }
        private async Task<int>CountCustomers()
        {
            using var context = _appDb.CreateDbContext();

            var Customers = from u in context.Users
                            from ur in context.UserRoles
                            where u.Id == ur.UserId
                            from r in context.Roles
                            where r.Id == ur.RoleId
                            where r.Name == "Customer"
                            select u;
            return  await Customers.CountAsync();
        }
        //private Task<ApiResponse<DashboardStatsDto>> OrderStats(DashboardStatsDto dto)
        //{

        //}
        private async Task<List<RecentCustomerDto>> CustomerStats()
        {
            using var context = _appDb.CreateDbContext();

            var Customers = from u in context.Users
                            from ur in context.UserRoles
                            where u.Id == ur.UserId
                            from r in context.Roles
                            where r.Id == ur.RoleId
                            where r.Name == "Customer"
                            select u;
            var CutomersStats =  await Customers.AsNoTracking().Select(x =>

                new RecentCustomerDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.UserName,
                    OrdersCount = x.Orders.Count,
                    RegisteredAt = x.CreatedAt
                }).ToListAsync();
            return CutomersStats;

        }

        public async Task<PaginatedResponse<ProductDto>> getActiveProduct(PaginationParams paginationParams)
        {
            var query =  db1.Products.AsNoTracking().AsQueryable();

            var totalCount = await query.CountAsync(x=>x.IsActive==true);
            var SkipPage = (paginationParams.Page - 1) * paginationParams.PageSize;

            var getPorduct =  await query.Where(x => x.IsActive == true)
                .Skip(SkipPage).Take(paginationParams.PageSize)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    SKU = x.SKU,
                    Slug = x.Slug,
                    BrandName = x.Brand.Name,
                    CategoryName = x.Category.Name,
                    Variants = x.Variants.Select(x => new ProductVariantDto
                    {
                        Size = x.Size,
                        StockQuantity = x.StockQuantity,
                        Color = x.Color
                    }).ToList()
                }).ToListAsync();
            return new PaginatedResponse<ProductDto>
            {
                Items = getPorduct,
                Page = paginationParams.Page,
                TotalItems = totalCount,
                HasNextPage = SkipPage + paginationParams.PageSize < totalCount,
                HasPreviousPage = SkipPage > 0,
                PageSize = paginationParams.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / paginationParams.PageSize)
            };
        }

        public async Task<ApiResponse<ProductDto>> MakeProductDeAcitve(int productId)
        {
            var product= await db1.Products.
                Include(x=>x.Brand)
                .Include(x=>x.Category)
                .Include(x=>x.Variants).
                Where(x => x.Id == productId && x.IsActive == true).FirstOrDefaultAsync();

            if (product == null)
                return new ApiResponse<ProductDto>
                    (
                         Success: false,
                        Message: "Not found Product ",
                        Data: null,
                        Errors: new[] { "not found product " },
                        TraceId: Guid.NewGuid().ToString()
                    );
          
            var productDto = new ProductDto
            {
                Id = product.Id,
                SKU = product.SKU,
                Slug = product.Slug,
                Name = product.Name,
                BrandName = product.Brand.Name,
                CategoryName = product.Category.Name,
                CreatedAt = product.CreatedAt,
                Price = product.Price,
                Description= product.Description,
                Variants = product.Variants.Select(x => new ProductVariantDto
                {
                    Size = x.Size,
                    StockQuantity = x.StockQuantity,
                    Color = x.Color
                }).ToList(),

            };
            product.IsActive = false;
            await db1.SaveChangesAsync();
            return new ApiResponse<ProductDto>
                  (
                       Success: true,
                      Message: "The product DeActive success ",
                      Data: productDto,
                      Errors: null,
                      TraceId: Guid.NewGuid().ToString()
                  );
        }
        public async Task<ApiResponse<ProductDto>> MakeProductAcitve(int productId)
        {
            var product= await db1.Products.
                Include(x=>x.Brand)
                .Include(x=>x.Category)
                .Include(x=>x.Variants).
                Where(x => x.Id == productId && x.IsActive == false).FirstOrDefaultAsync();

            if (product == null)
                return new ApiResponse<ProductDto>
                    (
                         Success: false,
                        Message: "Not found Product ",
                        Data: null,
                        Errors: new[] { "not found product " },
                        TraceId: Guid.NewGuid().ToString()
                    );
          
            var productDto = new ProductDto
            {
                Id = product.Id,
                SKU = product.SKU,
                Slug = product.Slug,
                Name = product.Name,
                BrandName = product.Brand.Name,
                CategoryName = product.Category.Name,
                CreatedAt = product.CreatedAt,
                Price = product.Price,
                Description= product.Description,
                Variants = product.Variants.Select(x => new ProductVariantDto
                {
                    Size = x.Size,
                    StockQuantity = x.StockQuantity,
                    Color = x.Color
                }).ToList(),

            };
            product.IsActive = true;
            await db1.SaveChangesAsync();
            return new ApiResponse<ProductDto>
                  (
                       Success: true,
                      Message: "The product DeActive success ",
                      Data: productDto,
                      Errors: null,
                      TraceId: Guid.NewGuid().ToString()
                  );
        }
       public async Task<ApiResponse<string>> UpdateQuantityProduct(int quantity,int pId,int pVariantsId)
        {
            var product = await db1.ProductVariants.Where(x => x.ProductId == pId &&x.Id==pVariantsId&& x.IsActive == true).FirstOrDefaultAsync();
            if (product ==null)
                return new ApiResponse<string>
                 (
                      Success: false,
                     Message: "Not found product",
                     Data: null,
                     Errors: new[] { product},
                     TraceId: Guid.NewGuid().ToString()
                 );
           product.StockQuantity= quantity;
            await db1.SaveChangesAsync();
            return new ApiResponse<string>
                (
                     Success: true,
                    Message: "The stock quantity of product has been updated",
                    Data: null,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                );
        }
       public async Task<string> UpdateUserAccountStatus(string userId, string userAccountStatus)
       {
            var checkUserAuth = await db1.Users.Where(x => x.Id == userId).FirstOrDefaultAsync() ;
            if (checkUserAuth != null)
            {
                // make is user banned
                if (checkUserAuth.AccountStatus != null)
                {
                    checkUserAuth.AccountStatus = userAccountStatus;
                    checkUserAuth.SecurityStampDate = DateTime.Now;
                    await db1.SaveChangesAsync();
                    return $"The user is {userAccountStatus} now";
                }
                //checkUserAuth.AccountStatus = "Suspension";
                //await db1.SaveChangesAsync();
                //return "The user is Suspension";
                if (checkUserAuth.AccountStatus == userAccountStatus)
                    return $"The user is {userAccountStatus} now";
            }
            return "The user not authorized";
        }


    }
}