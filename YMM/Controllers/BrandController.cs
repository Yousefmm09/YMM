using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpPost("AddBrand")]
        public async Task<IActionResult> AddBrand([FromBody] string brandName)
        {
            var brand = new YMM.Data.Entities.Brand
            {
                Name = brandName
            };
            var result = await _brandService.AddBrand(brand);
            return Ok(result);
        }
    }
}
