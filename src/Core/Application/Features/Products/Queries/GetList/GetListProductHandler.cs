using Application.Behaviors.Caching;
using Application.Repositories;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetProduct
{
    public class GetListProductHandler : IRequestHandler<GetListProductRequest, GetListResponse<GetListProductResponse>>
    {


        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetListProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }        

        public async Task<GetListResponse<GetListProductResponse>> Handle(GetListProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetList(index: request.PageRequest.Page, size: request.PageRequest.PageSize, cancellationToken: cancellationToken);
            return _mapper.Map<GetListResponse<GetListProductResponse>>(product);
        }
    }
}
