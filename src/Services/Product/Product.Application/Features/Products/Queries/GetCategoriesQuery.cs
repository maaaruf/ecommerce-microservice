using MediatR;
using Product.Application.Interfaces;

namespace Product.Application.Features.Products.Queries;

public class GetCategoriesQuery : IRequest<IEnumerable<string>>
{
}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<string>>
{
    private readonly IProductService _productService;

    public GetCategoriesQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IEnumerable<string>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetCategoriesAsync();
    }
} 