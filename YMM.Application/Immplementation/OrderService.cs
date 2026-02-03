using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Order;
using YMM.Application.Dto.Response;

namespace YMM.Application.Immplementation
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepo _OrderRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(IOrderRepo order,IHttpContextAccessor httpContext)
        {
            _OrderRepo = order;
            _httpContextAccessor = httpContext;
        }

        public Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto dto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authorized to perform this action.");
            }
            var order= _OrderRepo.CreateOrderAsync(userId, dto);
            return order;
        }

        public async Task<ApiResponse<OrderDto>> GetOrderDetailsAsync( int orderId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authorized to perform this action.");
            }
            var order= await _OrderRepo.GetOrderDetailsAsync(userId, orderId);
            return new ApiResponse<OrderDto>
                (
                Success: order != null ? true : false,
                Message: order != null ? "Order details retrieved successfully." : "Order not found.",
                Data: order,
                Errors: order != null ? null : new[] { "No order found with the provided ID." },
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<PaginatedResponse<OrderDto>> GetUserOrdersAsync(PaginationParams pagination, string status)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authorized to perform this action.");
            }
            var orders = await _OrderRepo.GetUserOrdersAsync(userId, pagination, status);
            return orders;
        }

        public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto)
        {
            var adminId=_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(adminId is null)
                throw new UnauthorizedAccessException("Admin is not authorized to perform this action.");
            var result= await _OrderRepo.UpdateOrderStatusAsync( orderId, dto, adminId);
            return result;
        }
    }
}
