using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;
using YMM.Application.Dto.Brand;
using YMM.Application.Dto.ProductDtos;

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
        public async Task<IActionResult> AddBrand([FromBody] CreatBrandDto dto)
        {

            var result = await _brandService.AddBrand(dto);
            return Ok(result);
        }
        [HttpGet("GetBrandById/{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var result = await _brandService.GetBrandById(id);
            return Ok(result);
        }
        [HttpDelete("DeleteBrand/{brandId}")]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            var result = await _brandService.DeleteBrand(brandId);
            return Ok(result);
        }
        [HttpPut("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand([FromBody] BrandFilterDto brand)
        {
            var result = await _brandService.UpdateBrand(brand);
            return Ok(result);
        }
    }
}
