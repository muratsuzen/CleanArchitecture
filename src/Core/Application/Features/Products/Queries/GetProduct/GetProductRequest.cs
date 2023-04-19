using MediatR;

namespace Application.Features.Products.Queries.GetProduct
{
    public record GetProductRequest() : IRequest<List<GetProductResponse>>;
}
