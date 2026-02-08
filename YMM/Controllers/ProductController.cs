using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.ProductDtos;

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
        [OutputCache(PolicyName = "ProductList")]
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
        [HttpGet("GetProductPagination")]
        [OutputCache(PolicyName = "ProductList")]
        public async Task<IActionResult> GetProductPagination([FromQuery] PaginationParams paginationParams)
        {
            var result = await _product.GetProductPagination(paginationParams);
            return Ok(result);
        }
        [HttpGet("GetProductbySlug")]
        public async Task<IActionResult> GetProductBySlug([FromQuery] string Name)
        {
            var res = await _product.GetProductbySlug(Name);
            return Ok(res);
        }
        [HttpGet("GetProductbySKU")]
        public async Task<IActionResult> GetProductBySKU([FromQuery] string Name)
        {
            var res = await _product.GetProductbySKU(Name);
            return Ok(res);
        }
        [HttpGet("GetProductbyCategoryName")]
        public async Task<IActionResult> GetProductByCategoryName([FromQuery] string Name)
        {
            var res = await _product.GetProductByCategory(Name);
            return Ok(res);
        }
        [HttpGet("GetProductFilterByFilter")]
        public async Task<IActionResult> GetProductFilterByFilter([FromQuery] PaginationParams paginationParams, [FromQuery] ProductFilterDto dto)
        {
            var res = await _product.GetProductFilterByFilter(paginationParams, dto);
            return Ok(res);
        }
        [HttpGet("GetProductSearchSuggestions")]
        public async Task<IActionResult> GetProductSearchSuggestions([FromQuery] string query)
        {
            var res = await _product.GetProductSearchSuggestions(query);
            return Ok(res);
        }
    }
}
