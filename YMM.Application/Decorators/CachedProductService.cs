using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.ProductDtos;
using YMM.Application.Dto.Response;
using YMM.Infrastructure.Caching;

namespace YMM.Application.Decorators
{
    /// <summary>
    /// Decorator pattern implementation for caching product service calls
    /// Reduces database load and improves response times for frequently accessed data
    /// </summary>
    public class CachedProductService : IProduct
    {
        private readonly IProduct _innerService;
        private readonly ICacheService _cacheService;
        private const string ProductCacheKeyPrefix = "product:";
        private const string ProductListCacheKey = "products:all";
        private const string ProductPaginationCacheKeyPrefix = "products:page:";

        public CachedProductService(IProduct innerService, ICacheService cacheService)
        {
            _innerService = innerService;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<ProductDto>> Add(AddProductDto dto)
        {
            var result = await _innerService.Add(dto);
            
            // Invalidate relevant caches on add
            if (result.Success)
            {
                await _cacheService.RemoveAsync(ProductListCacheKey);
                await _cacheService.RemoveByPrefixAsync(ProductPaginationCacheKeyPrefix);
            }
            
            return result;
        }

        public async Task<string> Delete(int productId)
        {
            var result = await _innerService.Delete(productId);
            
            // Invalidate caches on delete
            await _cacheService.RemoveAsync($"{ProductCacheKeyPrefix}{productId}");
            await _cacheService.RemoveAsync(ProductListCacheKey);
            await _cacheService.RemoveByPrefixAsync(ProductPaginationCacheKeyPrefix);
            
            return result;
        }

        public async Task<List<ProductDto>> GetAll()
        {
            return await _cacheService.GetOrCreateAsync(
                ProductListCacheKey,
                () => _innerService.GetAll(),
                TimeSpan.FromMinutes(10));
        }

        public async Task<ProductDto> GetById(int productId)
        {
            var cacheKey = $"{ProductCacheKeyPrefix}{productId}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetById(productId),
                TimeSpan.FromMinutes(30));
        }

        public async Task<ApiResponse<PaginatedResponse<ProductDetailDto>>> GetProductPagination(PaginationParams paginationParams)
        {
            var cacheKey = $"{ProductPaginationCacheKeyPrefix}{paginationParams.Page}:{paginationParams.PageSize}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetProductPagination(paginationParams),
                TimeSpan.FromMinutes(5));
        }

        public async Task<ApiResponse<ProductDto>> GetProductbySlug(string slug)
        {
            var cacheKey = $"{ProductCacheKeyPrefix}slug:{slug}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetProductbySlug(slug),
                TimeSpan.FromMinutes(30));
        }

        public async Task<ApiResponse<ProductDto>> GetProductbySKU(string sku)
        {
            var cacheKey = $"{ProductCacheKeyPrefix}sku:{sku}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetProductbySKU(sku),
                TimeSpan.FromMinutes(30));
        }

        public async Task<ApiResponse<List<ProductDto>>> GetProductByCategory(string categoryName)
        {
            var cacheKey = $"{ProductCacheKeyPrefix}category:{categoryName}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetProductByCategory(categoryName),
                TimeSpan.FromMinutes(15));
        }

        public async Task<UpdateProductDto> UpdateProductDto(UpdateProductDto dto, int productId)
        {
            var result = await _innerService.UpdateProductDto(dto, productId);
            
            // Invalidate caches on update
            await _cacheService.RemoveAsync($"{ProductCacheKeyPrefix}{productId}");
            await _cacheService.RemoveAsync(ProductListCacheKey);
            await _cacheService.RemoveByPrefixAsync(ProductPaginationCacheKeyPrefix);
            
            return result;
        }
        public async Task<List<ApiResponse<ProductFiltersResponseDto>>> GetProductFilterByFilter(PaginationParams pagination, ProductFilterDto dto)
        {
            // Caching can be complex for filter-based queries; skipping cache for simplicity
            var cacheKey = $"{ProductPaginationCacheKeyPrefix}{pagination.Page}:{pagination.PageSize}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetProductFilterByFilter(pagination,dto),
                TimeSpan.FromMinutes(5));
        }
        public async Task<ApiResponse<List<ProductSearchSuggestionDto>>> GetProductSearchSuggestions(string query)
        {
            var cacheKey = $"{ProductCacheKeyPrefix}search:{query}";
            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _innerService.GetProductSearchSuggestions(query),
                TimeSpan.FromMinutes(10));
        }
    }
}
