using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductRequest(string Name,string Barcode,decimal Price) : IRequest<CreateProductResponse>;
}
