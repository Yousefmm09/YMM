using Microsoft.EntityFrameworkCore;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;

namespace YMM.Api.Middleware
{
    public class RequestTrackMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTrackMiddleware> _logger;

        public RequestTrackMiddleware(
            RequestDelegate next,
            ILogger<RequestTrackMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, AppDb db)
        {
            try
            {
                // Track Category Views
                if (context.Request.Method == HttpMethods.Get &&
                    context.Request.Path.StartsWithSegments("/api/Category", StringComparison.OrdinalIgnoreCase))
                {
                    // Check if it's a single category request (has id parameter)
                    if (context.Request.RouteValues.TryGetValue("id", out var idValue))
                    {
                        if (int.TryParse(idValue?.ToString(), out int categoryId))
                        {
                            await IncrementCategoryViewCount(db, categoryId);
                        }
                    }
                }

                // Track Product Views (يمكنك إضافة tracking للمنتجات كمان)
                if (context.Request.Method == HttpMethods.Get &&
                    context.Request.Path.StartsWithSegments("/api/products", StringComparison.OrdinalIgnoreCase))
                {
                    if (context.Request.RouteValues.TryGetValue("id", out var idValue))
                    {
                        if (int.TryParse(idValue?.ToString(), out int productId))
                        {
                            await IncrementProductViewCount(db, productId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't break the request
                _logger.LogError(ex, "Error in RequestTrackMiddleware");
            }
            finally
            {
                // Always continue to the next middleware
                await _next(context);
            }
        }

        private async Task IncrementCategoryViewCount(AppDb db, int categoryId)
        {
            try
            {
                var category = await db.Categories
                    .FirstOrDefaultAsync(x => x.Id == categoryId);

                if (category != null)
                {
                    category.ViewCount++;
                    await db.SaveChangesAsync();

                    _logger.LogInformation(
                        "Category view tracked: ID={CategoryId}, NewCount={ViewCount}",
                        categoryId,
                        category.ViewCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing category view count for ID: {CategoryId}", categoryId);
            }
        }

        private async Task IncrementProductViewCount(AppDb db, int productId)
        {
            try
            {
                var product = await db.Products
                    .FirstOrDefaultAsync(x => x.Id == productId);

                if (product != null)
                {
                    product.ViewCount++;
                    await db.SaveChangesAsync();

                    _logger.LogInformation(
                        "Product view tracked: ID={ProductId}, NewCount={ViewCount}",
                        productId,
                        product.ViewCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing product view count for ID: {ProductId}", productId);
            }
        }
    }
}