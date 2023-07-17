using MediatR;

namespace Application.Features.Products.Queries.GetProduct
{
    public sealed record GetProductRequest() : IRequest<List<GetProductResponse>>;
}
