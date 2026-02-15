using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Admin;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.ProductDtos;
using YMM.Application.Dto.Response;

namespace YMM.Application.Abstract.Repositories
{
    public interface IAdminRepo
    {
        Task<ApiResponse<DashboardStatsDto>> DashboardStats();
        Task<PaginatedResponse<ProductDto>> getActiveProduct(PaginationParams paginationParams);
        Task<ApiResponse<ProductDto>> MakeProductDeAcitve(int productId);
        Task<ApiResponse<ProductDto>> MakeProductAcitve(int productId);
        Task<ApiResponse<string>> UpdateQuantityProduct(int quantity, int pId, int pVariantsId);
        Task<string> UpdateUserAccountStatus(string userId, string userAccountStatus);
    }
}
