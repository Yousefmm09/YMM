using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;
using YMM.Application.Dto.Category;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Product;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _Categories;
        public CategoryController(ICategoryService categories)
        {
            _Categories = categories;
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {

            var result = await _Categories.AddCategory(category);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _Categories.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound(new
                {
                    Message = "Category not found"
                });

            return Ok(category);
        }
        //[HttpGet("Categories")]
        //public async Task<IActionResult> GetAllCategories()
        //{
        //    var result = await _Categories.GetAllCategoriesAsync();
        //    return Ok(result);
        //}
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _Categories.DeleteCategory(id);
            return Ok(result);
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto category)
        {
            var result = await _Categories.Update(category);
            return Ok(result);
        }
        [HttpGet("CategoryPagination")]
        public async Task<IActionResult> GetCategoryPagination([FromQuery] PaginationParams paginationParams)
        {
            var result = await _Categories.GetCategoryPagination(paginationParams);
            return Ok(result);
        }
    }
}
