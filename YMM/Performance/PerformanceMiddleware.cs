using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace YMM.Api.Performance
{
    /// <summary>
    /// Middleware to track request performance metrics
    /// Implements performance monitoring as per dotnet-performance-analyst guidelines
    /// </summary>
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMiddleware> _logger;
        private readonly bool _logAllRequests;
        private const int SlowRequestThresholdMs = 100; // Lower threshold to see more requests (development only)

        public PerformanceMiddleware(
            RequestDelegate next, 
            ILogger<PerformanceMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _logAllRequests = environment.IsDevelopment(); // Only log all in Development
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;

            // Register callback to add header before response starts
            context.Response.OnStarting(() =>
            {
                stopwatch.Stop();
                var elapsedMs = stopwatch.ElapsedMilliseconds;
                
                // Add performance header before response starts
                if (!context.Response.Headers.ContainsKey("X-Response-Time-Ms"))
                {
                    context.Response.Headers["X-Response-Time-Ms"] = elapsedMs.ToString();
                }
                
                return Task.CompletedTask;
            });

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsedMs = stopwatch.ElapsedMilliseconds;

                // Log ALL requests in development, only slow ones in production
                if (_logAllRequests)
                {
                    _logger.LogInformation(
                        "Request: {Method} {Path} completed in {ElapsedMs}ms with status {StatusCode}",
                        requestMethod,
                        requestPath,
                        elapsedMs,
                        context.Response.StatusCode);
                }

                // Always log slow requests as WARNING for easy filtering
                if (elapsedMs > SlowRequestThresholdMs)
                {
                    _logger.LogWarning(
                        "⚠️ SLOW Request: {Method} {Path} completed in {ElapsedMs}ms with status {StatusCode}",
                        requestMethod,
                        requestPath,
                        elapsedMs,
                        context.Response.StatusCode);
                }
            }
        }
    }
}
