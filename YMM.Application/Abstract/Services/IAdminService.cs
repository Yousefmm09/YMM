using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Admin;
using YMM.Application.Dto.Response;

namespace YMM.Application.Abstract.Services
{
    public interface IAdminService
    {
        public Task<ApiResponse<DashboardStatsDto>> DashboardStats();
    }
}
