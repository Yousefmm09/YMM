using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using YMM.Api.Performance;

namespace YMM.Api.Controllers
{
    /// <summary>
    /// Controller for testing and monitoring performance
    /// Provides endpoints for load testing and metrics validation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceTestController : ControllerBase
    {
        private readonly PerformanceMetrics _metrics;

        public PerformanceTestController(PerformanceMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// Test endpoint for CPU-bound operations
        /// </summary>
        [HttpGet("cpu-test")]
        public IActionResult CpuTest([FromQuery] int iterations = 1000)
        {
            var result = 0;
            for (int i = 0; i < iterations; i++)
            {
                result += i * i;
            }
            return Ok(new { Result = result, Iterations = iterations });
        }

        /// <summary>
        /// Test endpoint for memory allocations
        /// </summary>
        [HttpGet("memory-test")]
        public IActionResult MemoryTest([FromQuery] int size = 1000)
        {
            var list = new List<string>();
            for (int i = 0; i < size; i++)
            {
                list.Add($"Item {i}");
            }
            return Ok(new { Count = list.Count });
        }

        /// <summary>
        /// Test endpoint with output caching
        /// </summary>
        [HttpGet("cached-test")]
        [OutputCache(Duration = 60)]
        public IActionResult CachedTest()
        {
            return Ok(new
            {
                Timestamp = DateTime.UtcNow,
                Message = "This response is cached for 60 seconds"
            });
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }
    }
}
