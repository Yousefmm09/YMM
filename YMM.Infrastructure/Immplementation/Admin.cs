using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Admin;
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
        public Admin(IDbContextFactory<AppDb> db)
        {
         _appDb = db;   
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
             await Customers.AsNoTracking().ToListAsync();
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
    }
}
