using MediatR;
using Product.Application.Interfaces;
using Shared.Contracts.Models;

namespace Product.Application.Features.Products.Queries;

public class GetProductsByCategoryQuery : IRequest<IEnumerable<ProductDto>>
{
    public string Category { get; set; } = string.Empty;
}

public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, IEnumerable<ProductDto>>
{
    private readonly IProductService _productService;

    public GetProductsByCategoryQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetProductsByCategoryAsync(request.Category);
    }
} 