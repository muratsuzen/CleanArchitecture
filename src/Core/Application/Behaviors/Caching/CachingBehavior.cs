using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
            TResponse response;
            if (request.BypassCache) return await next();

            async Task<TResponse> GetResponseAddToCache() {
                response = await next();
                var slidingExpration = request.SlidingExpiration == null ? TimeSpan.FromHours(_cacheSetting.SlidingExpiration) : request.SlidingExpiration;
                var options = 
            }
        }
    }
}
