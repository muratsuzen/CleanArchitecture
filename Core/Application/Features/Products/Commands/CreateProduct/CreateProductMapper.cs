using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductMapper : Profile
    {
        public CreateProductMapper()
        {
            CreateMap<CreateProductRequest,Product>().ReverseMap();
            CreateMap<CreateProductResponse,Product>().ReverseMap();
        }
    }
}
