using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Products.Commands;
using Product.Application.Features.Products.Queries;
using Shared.Contracts.Models;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] bool activeOnly = true)
    {
        _logger.LogInformation("Getting products with activeOnly: {ActiveOnly}", activeOnly);
        
        var query = new GetProductsQuery { ActiveOnly = activeOnly };
        var products = await _mediator.Send(query);
        
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(string id)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", id);
        
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query);
        
        return Ok(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
    {
        _logger.LogInformation("Creating new product: {ProductName}", request.Name);
        
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        var command = new CreateProductCommand
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category,
            Tags = request.Tags,
            ImageUrls = request.ImageUrls,
            CreatedBy = userId
        };

        var product = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<ProductDto>> UpdateProduct(string id, [FromBody] UpdateProductRequest request)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);
        
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        var command = new UpdateProductCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category,
            Tags = request.Tags,
            ImageUrls = request.ImageUrls,
            IsActive = request.IsActive,
            UpdatedBy = userId
        };

        var product = await _mediator.Send(command);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);
        
        var command = new DeleteProductCommand { Id = id };
        await _mediator.Send(command);
        
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<ProductSearchResponse>> SearchProducts([FromQuery] ProductSearchRequest request)
    {
        _logger.LogInformation("Searching products with query: {Query}, category: {Category}", 
            request.SearchTerm, request.Category);
        
        var query = new SearchProductsQuery { Request = request };
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        _logger.LogInformation("Getting product categories");
        
        var query = new GetCategoriesQuery();
        var categories = await _mediator.Send(query);
        
        return Ok(categories);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(string category)
    {
        _logger.LogInformation("Getting products by category: {Category}", category);
        
        var query = new GetProductsByCategoryQuery { Category = category };
        var products = await _mediator.Send(query);
        
        return Ok(products);
    }
} 