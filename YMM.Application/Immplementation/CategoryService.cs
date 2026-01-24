using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Product;
using YMM.Data.Entities;

namespace YMM.Application.Immplementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _category;
        public CategoryService(ICategoryRepo categories)
        {
            _category = categories;
        }
        public async Task<string> AddCategory(CategoryDto categoryName)
        {
            var categoryNameExist = new Category
            {
                Name = categoryName.Name,
                Description = categoryName.Description,
                Slug = categoryName.Slug
            };
            var category = await _category.AddAsync(categoryNameExist);
            if(category == null) 
                return "Failed to add category";
            return "Category added successfully";  
        }
    }
}
