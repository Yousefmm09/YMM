using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Order;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto dto)
        {
            
            var result = await _orderService.CreateOrderAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("GetOrderPagination")]
        public async Task<IActionResult> GetUserOrders([FromQuery]PaginationParams pagination, [FromQuery]string status)
        {
            var result = await _orderService.GetUserOrdersAsync(pagination, status);
            return Ok(result);
        }
        [HttpGet("details/{orderId}")]
        public async Task<IActionResult> GetOrderDetails([FromRoute]int orderId)
        {
            var result = await _orderService.GetOrderDetailsAsync(orderId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPut("update-status/{orderId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute]int orderId, [FromBody]UpdateOrderStatusDto dto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
