using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YMM.Application.Abstract.Services;
using YMM.Application.Immplementation;
using YMM.Infrastructure.Caching;

namespace YMM.Application
{
    public static class ModuleApplicationRegister
    {
        public static IServiceCollection ApplicationRegist(this  IServiceCollection service)
        {
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<IEmailService, EmailService>();
            service.AddTransient<IProduct, ProductSerivce>();
            service.AddTransient<IBrandService, BrandService>();
            service.AddTransient<ICategoryService, CategoryService>();
            service.AddTransient<ICartService, CartService>();
            service.AddTransient<IOrderService, OrderService>();
            service.AddTransient<IAddressService, AdressService>();
            service.AddScoped<ICacheService, MemoryCacheService>();
            service.AddTransient<IReviewService, ReviewService>(); 
            service.AddTransient<IWishListService, WishListService>();
            return service;
        }

    }
}
