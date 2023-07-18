using Application.Behaviors.Caching;
using Application.Requests;
using Application.Responses;
using MediatR;

namespace Application.Features.Products.Queries.GetProduct
{
    public sealed record GetListProductRequest() : IRequest<GetListResponse<GetListProductResponse>>, ICachableRequest
    {
        public PageRequest PageRequest { get; set; }
        public bool BypassCache { get; }
        public string CacheKey => $"GetListProduct";
        public TimeSpan? SlidingExpiration { get; }
    }
}
