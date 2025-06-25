using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Models;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        _logger.LogInformation("Getting all products");
        
        // TODO: Implement actual product service
        var products = new List<ProductDto>
        {
            new ProductDto
            {
                Id = "1",
                Name = "Sample Product 1",
                Description = "This is a sample product description",
                Price = 99.99m,
                Category = "Electronics",
                StockQuantity = 100,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductDto
            {
                Id = "2",
                Name = "Sample Product 2",
                Description = "Another sample product description",
                Price = 149.99m,
                Category = "Books",
                StockQuantity = 50,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(string id)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", id);
        
        // TODO: Implement actual product service
        var product = new ProductDto
        {
            Id = id,
            Name = $"Product {id}",
            Description = $"Description for product {id}",
            Price = 99.99m,
            Category = "Electronics",
            StockQuantity = 100,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
    {
        _logger.LogInformation("Creating new product: {ProductName}", request.Name);
        
        // TODO: Implement actual product service
        var product = new ProductDto
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            StockQuantity = request.StockQuantity,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(string id, [FromBody] UpdateProductRequest request)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);
        
        // TODO: Implement actual product service
        var product = new ProductDto
        {
            Id = id,
            Name = request.Name ?? "Updated Product",
            Description = request.Description ?? "Updated description",
            Price = request.Price ?? 99.99m,
            Category = request.Category ?? "Electronics",
            StockQuantity = request.StockQuantity ?? 100,
            IsActive = request.IsActive ?? true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);
        
        // TODO: Implement actual product service
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string? query, [FromQuery] string? category)
    {
        _logger.LogInformation("Searching products with query: {Query}, category: {Category}", query, category);
        
        // TODO: Implement actual search service
        var products = new List<ProductDto>
        {
            new ProductDto
            {
                Id = "1",
                Name = "Search Result Product",
                Description = "This is a search result",
                Price = 99.99m,
                Category = category ?? "Electronics",
                StockQuantity = 100,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        return Ok(products);
    }
} 