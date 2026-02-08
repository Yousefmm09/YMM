using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;

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
    }
}
