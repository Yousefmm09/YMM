using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Order;
using YMM.Application.Dto.Response;

namespace YMM.Application.Abstract.Services
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto dto);
        Task<PaginatedResponse<OrderDto>> GetUserOrdersAsync( PaginationParams pagination, string status);
        Task<ApiResponse<OrderDto>> GetOrderDetailsAsync(int orderId);
        Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto);
    }
}
