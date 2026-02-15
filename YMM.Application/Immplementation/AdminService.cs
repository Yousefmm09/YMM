using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Admin;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.ProductDtos;
using YMM.Application.Dto.Response;

namespace YMM.Application.Immplementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepo _admin;
        private readonly IHttpContextAccessor _httpContext;
        public AdminService(IAdminRepo admin,IHttpContextAccessor httpContext)
        {
            _admin = admin;
            _httpContext = httpContext;
        }
        public Task<ApiResponse<DashboardStatsDto>> DashboardStats()
        {
            var res= _admin.DashboardStats();
            return res;
        }

        public Task<PaginatedResponse<ProductDto>> getActiveProduct(PaginationParams paginationParams)
        {
            var res=_admin.getActiveProduct(paginationParams);
            return res;
        }

        public Task<ApiResponse<ProductDto>> MakeProductDeAcitve(int productId)
        {
            var res=_admin.MakeProductDeAcitve(productId);
            return res;
        }
        public Task<ApiResponse<ProductDto>> MakeProductAcitve(int productId)
        {
            var res=_admin.MakeProductAcitve(productId);
            return res;
        }
        public Task<ApiResponse<string>> UpdateQuantityProduct(int quantity, int pId, int pVariantsId)
        {
            var res=_admin.UpdateQuantityProduct(quantity, pId,pVariantsId);
            return res;
        }
        public Task<string> UpdateUserAccountStatus(string userId,string userAccountStatus)
        {
            var res=_admin.UpdateUserAccountStatus(userId, userAccountStatus);
            return res;
        }
    }
}
