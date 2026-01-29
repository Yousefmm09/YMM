using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Order;
using YMM.Application.Dto.Response;

namespace YMM.Application.Abstract.Repositories
{
    public interface IOrderRepo
    {
        Task<ApiResponse<OrderDto>> CreateOrderAsync(string userId, CreateOrderDto dto);
        Task<PaginatedResponse<OrderDto>> GetUserOrdersAsync(string userId, PaginationParams pagination, string status);
        Task<OrderDto> GetOrderDetailsAsync(string userId, int orderId);
        Task<ApiResponse<OrderDto>> CancelOrderAsync(string userId, int orderId, CancelOrderDto dto);

        // Admin endpoints
        Task<ApiResponse<PaginatedResponse<OrderDto>>> GetAllOrdersAsync(OrderFilterDto filter);
        Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto, string adminUserId);
        //Task<ApiResponse<OrderStatsDto>> GetOrderStatsAsync();
    }
}
