using Application.Paging;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetProduct
{
    public class GetListProductMapper : Profile
    {
        public GetListProductMapper()
        {
            CreateMap<GetListProductRequest,Product>().ReverseMap();
            CreateMap<GetListProductResponse,Product>().ReverseMap();
            CreateMap<IPaginate<Product>, GetListResponse<GetListProductResponse>>().ReverseMap();
        }
    }
}
