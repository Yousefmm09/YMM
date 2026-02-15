using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Common;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _admin;
        public AdminController(IAdminService service)
        {
            _admin = service;
        }
        [HttpGet("Dashboard/Stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            if (ModelState.IsValid)
            {
                var res = await _admin.DashboardStats();
                return Ok(res);
            }
            return BadRequest(ModelState);
        }
        [HttpPatch("Product/{id}/deactive")]
        public async Task<IActionResult> MakeProductDeAcitve([FromRoute]int id)
        {
            if (ModelState.IsValid)
            {
                var res = await _admin.MakeProductDeAcitve(id);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }
        [HttpPatch("Product/{id}/active")]
        public async Task<IActionResult> MakeProductAcitve([FromRoute]int id)
        {
            if (ModelState.IsValid)
            {
                var res = await _admin.MakeProductAcitve(id);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }
        [HttpGet("Product/GetActive")]
        public async Task<IActionResult> GetActiveProduct([FromQuery]PaginationParams paginationParams)
        {
            if (ModelState.IsValid)
            {
                var res = await _admin.getActiveProduct(paginationParams);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }
        [HttpPatch("Product/{id}/ProductVariant/{vId}/update/quantity/{quantity}")]
        public async Task<IActionResult> GetActiveProduct([FromRoute]int id ,[FromRoute]int quantity, [FromRoute] int vId)
        {
            if (ModelState.IsValid)
            {
                var res = await _admin.UpdateQuantityProduct(quantity,id,vId);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }
        [HttpPatch("Account/status")]
        public async Task<IActionResult> UpdateAccountUserStatus([FromForm] string status,string userId)
        {
            if (ModelState.IsValid)
            {
                var res = await _admin.UpdateUserAccountStatus(userId,status);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }
    }
}
