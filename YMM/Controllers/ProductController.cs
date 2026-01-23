using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Product;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;
        public ProductController(IProduct product)
        {
            _product = product;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddProductDto dto)
        {
            var result = await _product.Add(dto);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _product.GetAll();
            return Ok(result);
        }
        [HttpDelete("Delete/{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _product.Delete(productId);
            return Ok(result);
        }
        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto dto, int productId)
        {
            var result = await _product.UpdateProductDto(dto, productId);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _product.GetById(id);
            return Ok(result);
        }

    }
}
