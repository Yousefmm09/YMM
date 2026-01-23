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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _Categories;
        public CategoryController(ICategoryRepo categories)
        {
            _Categories = categories;
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] string categoryName)
        {
            var category = new YMM.Data.Entities.Category
            {
                Name = categoryName
            };
            var result =  await _Categories.AddAsync(category);
            return Ok(result);
        }
    }
}
