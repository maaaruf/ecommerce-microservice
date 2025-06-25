using MediatR;
using Shared.Contracts.Models;

namespace Product.Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public List<string> ImageUrls { get; set; } = new();
    public string CreatedBy { get; set; } = string.Empty;
} 