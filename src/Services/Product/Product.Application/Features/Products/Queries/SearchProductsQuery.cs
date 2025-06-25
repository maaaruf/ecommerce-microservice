using MediatR;
using Product.Application.Interfaces;
using Shared.Contracts.Models;

namespace Product.Application.Features.Products.Queries;

public class SearchProductsQuery : IRequest<ProductSearchResponse>
{
    public ProductSearchRequest Request { get; set; } = new();
}

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, ProductSearchResponse>
{
    private readonly IProductService _productService;

    public SearchProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductSearchResponse> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productService.SearchProductsAsync(request.Request);
    }
} 