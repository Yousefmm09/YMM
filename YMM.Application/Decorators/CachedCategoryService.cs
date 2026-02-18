using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Category;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Response;
using YMM.Infrastructure.Caching;
using YMM.Infrastructure.Context.Config;

namespace YMM.Application.Decorators
{
    /// <summary>
    /// Decorator for caching category service operations
    /// </summary>
    public class CachedCategoryService : ICategoryService
    {
        private readonly ICategoryService _innerService;
        private readonly ICacheService _cacheService;
        private const string CategoryCacheKeyPrefix = "category:";
        private const string CategoryPaginationCacheKeyPrefix = "categories:page:";

        public CachedCategoryService(ICategoryService innerService, ICacheService cacheService)
        {
            _innerService = innerService;
            _cacheService = cacheService;
        }

        public async Task<CategoryResponse> AddCategory(CreatCategoryDto categoryName)
        {
            var result = await _innerService.AddCategory(categoryName);
            await _cacheService.RemoveByPrefixAsync(CategoryPaginationCacheKeyPrefix);
            return result;
        }

        public async Task<ApiResponse<string>> DeleteCategory(int id)
        {
            var result = await _innerService.DeleteCategory(id);
            
            if (result.Success)
            {
                await _cacheService.RemoveAsync($"{CategoryCacheKeyPrefix}{id}");
                await _cacheService.RemoveByPrefixAsync(CategoryPaginationCacheKeyPrefix);
            }
            
            return result;
        }

        public async Task<ApiResponse<CategoryDetailsDto?>> GetCategoryByIdAsync(int id)
        {
            var cacheKey = $"{CategoryCacheKeyPrefix}{id}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetCategoryByIdAsync(id),
                TimeSpan.FromMinutes(30));
        }

        public async Task<ApiResponse<PaginatedResponse<CategoryDetailsDto>>> GetCategoryPagination(PaginationParams pagination)
        {
            var cacheKey = $"{CategoryPaginationCacheKeyPrefix}{pagination.Page}:{pagination.PageSize}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetCategoryPagination(pagination),
                TimeSpan.FromMinutes(10));
        }

        public async Task<ApiResponse<CategoryDto>> Update(CategoryDto category)
        {
            var result = await _innerService.Update(category);
            
            if (result.Success)
            {
                await _cacheService.RemoveAsync($"{CategoryCacheKeyPrefix}{category.Id}");
                await _cacheService.RemoveByPrefixAsync(CategoryPaginationCacheKeyPrefix);
            }
            
            return result;
        }
    }
}
