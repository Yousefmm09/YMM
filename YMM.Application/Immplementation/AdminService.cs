using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Admin;
using YMM.Application.Dto.Response;

namespace YMM.Application.Immplementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepo _admin;
        public AdminService(IAdminRepo admin)
        {
            _admin = admin;
        }
        public Task<ApiResponse<DashboardStatsDto>> DashboardStats()
        {
            var res= _admin.DashboardStats();
            return res;
        }
    }
}
