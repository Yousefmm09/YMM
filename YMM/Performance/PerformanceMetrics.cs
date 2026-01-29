using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace YMM.Api.Performance
{
    /// <summary>
    /// Performance metrics tracking using System.Diagnostics.Metrics
    /// Provides instrumentation for performance analysis and monitoring
    /// </summary>
    public class PerformanceMetrics
    {
        private readonly Meter _meter;
        private readonly Counter<long> _requestCounter;
        private readonly Histogram<double> _requestDuration;
        private readonly Counter<long> _dbQueryCounter;
        private readonly Histogram<double> _dbQueryDuration;
        private readonly Counter<long> _cacheHitCounter;
        private readonly Counter<long> _cacheMissCounter;

        public PerformanceMetrics()
        {
            _meter = new Meter("YMM.Api", "1.0.0");
            
            _requestCounter = _meter.CreateCounter<long>(
                "http_requests_total",
                description: "Total number of HTTP requests");

            _requestDuration = _meter.CreateHistogram<double>(
                "http_request_duration_ms",
                unit: "ms",
                description: "Duration of HTTP requests in milliseconds");

            _dbQueryCounter = _meter.CreateCounter<long>(
                "db_queries_total",
                description: "Total number of database queries");

            _dbQueryDuration = _meter.CreateHistogram<double>(
                "db_query_duration_ms",
                unit: "ms",
                description: "Duration of database queries in milliseconds");

            _cacheHitCounter = _meter.CreateCounter<long>(
                "cache_hits_total",
                description: "Total number of cache hits");

            _cacheMissCounter = _meter.CreateCounter<long>(
                "cache_misses_total",
                description: "Total number of cache misses");
        }

        public void RecordRequest(string method, string path, double durationMs, int statusCode)
        {
            _requestCounter.Add(1, 
                new KeyValuePair<string, object?>("method", method),
                new KeyValuePair<string, object?>("path", path),
                new KeyValuePair<string, object?>("status", statusCode));

            _requestDuration.Record(durationMs,
                new KeyValuePair<string, object?>("method", method),
                new KeyValuePair<string, object?>("path", path));
        }

        public void RecordDbQuery(string queryType, double durationMs)
        {
            _dbQueryCounter.Add(1, new KeyValuePair<string, object?>("type", queryType));
            _dbQueryDuration.Record(durationMs, new KeyValuePair<string, object?>("type", queryType));
        }

        public void RecordCacheHit(string key) => _cacheHitCounter.Add(1, new KeyValuePair<string, object?>("key", key));
        
        public void RecordCacheMiss(string key) => _cacheMissCounter.Add(1, new KeyValuePair<string, object?>("key", key));
    }
}
