using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Application.Behaviors.Caching
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICachableRequest
    {

        private readonly IDistributedCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
        private readonly CacheSettings _cacheSetting;

        public CachingBehavior(ILogger<CachingBehavior<TRequest, TResponse>> logger, IDistributedCache cache, IConfiguration configuration)
        {
            _logger = logger;
            _cache = cache;
            _cacheSetting = configuration.GetSection("CacheSettings").Get<CacheSettings>();
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.BypassCache) return await next();

            TResponse response;



            byte[]? cachedResponse = await _cache.GetAsync((string)request.CacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse))!;
                _logger.LogInformation($"Fetched from Cache -> {request.CacheKey}");
            }
            else
            {
                response = await GetResponseAddToCache(request, next, cancellationToken);
            }
            return response;
        }

        private async Task<TResponse> GetResponseAddToCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response = await next();
            var slidingExpration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSetting.SlidingExpiration);
            var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpration };
            var serializedData = Encoding.Default.GetBytes(JsonSerializer.Serialize(response));

            await _cache.SetAsync(request.CacheKey, serializedData, options, cancellationToken);
            _logger.LogInformation($"Added to Cache -> {request.CacheKey}");

            return response;
        }

    }
}
