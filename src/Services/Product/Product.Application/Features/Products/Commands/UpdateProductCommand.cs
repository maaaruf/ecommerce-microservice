using MediatR;
using Product.Application.Interfaces;
using Shared.Contracts.Models;

namespace Product.Application.Features.Products.Commands;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? StockQuantity { get; set; }
    public string? Category { get; set; }
    public List<string>? Tags { get; set; }
    public List<string>? ImageUrls { get; set; }
    public bool? IsActive { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductService _productService;

    public UpdateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = new UpdateProductRequest
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category,
            Tags = request.Tags,
            ImageUrls = request.ImageUrls,
            IsActive = request.IsActive
        };

        return await _productService.UpdateProductAsync(request.Id, updateRequest, request.UpdatedBy);
    }
} 