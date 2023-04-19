using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetProduct
{
    public class GetProductMapper : Profile
    {
        public GetProductMapper()
        {
            CreateMap<GetProductRequest,Product>().ReverseMap();
            CreateMap<GetProductResponse,Product>().ReverseMap();
        }
    }
}
