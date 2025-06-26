using MediatR;
using Product.Application.Interfaces;
using Shared.Contracts.Models;

namespace Product.Application.Features.Products.Queries;

public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
{
    public bool ActiveOnly { get; set; } = true;
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductService _productService;

    public GetProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return request.ActiveOnly 
            ? await _productService.GetActiveProductsAsync()
            : await _productService.GetAllProductsAsync();
    }
} 