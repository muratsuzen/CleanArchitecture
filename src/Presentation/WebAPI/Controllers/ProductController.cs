using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetProduct;
using Application.Requests;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateProductResponse>> Post(CreateProductRequest request,CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request,cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetListProductRequest>>> Get([FromQuery] PageRequest request, CancellationToken cancellationToken)
        {
            GetListResponse<GetListProductResponse> response = await mediator.Send(new GetListProductRequest { PageRequest = request},cancellationToken : cancellationToken);
            return Ok(response);
        }
    }
}
