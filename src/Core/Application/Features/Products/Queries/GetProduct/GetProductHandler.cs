using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductRequest, List<GetProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<GetProductResponse>> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAll(cancellationToken);
            return _mapper.Map<List<GetProductResponse>>(product);
        }
    }
}
