using MediatR;
using Product.Application.Interfaces;
using Shared.Contracts.Models;

namespace Product.Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public string Id { get; set; } = string.Empty;
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductService _productService;

    public GetProductByIdQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetProductByIdAsync(request.Id);
    }
} 