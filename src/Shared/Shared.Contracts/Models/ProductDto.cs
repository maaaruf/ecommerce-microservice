using System.ComponentModel.DataAnnotations;

namespace Shared.Contracts.Models;

public class ProductDto
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
    
    public string Category { get; set; } = string.Empty;
    
    public List<string> Tags { get; set; } = new();
    
    public List<string> ImageUrls { get; set; } = new();
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}

public class CreateProductRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
    
    public string Category { get; set; } = string.Empty;
    
    public List<string> Tags { get; set; } = new();
    
    public List<string> ImageUrls { get; set; } = new();
}

public class UpdateProductRequest
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }
    
    [Range(0, int.MaxValue)]
    public int? StockQuantity { get; set; }
    
    public string? Category { get; set; }
    
    public List<string>? Tags { get; set; }
    
    public List<string>? ImageUrls { get; set; }
    
    public bool? IsActive { get; set; }
}

public class ProductSearchRequest
{
    public string? SearchTerm { get; set; }
    
    public string? Category { get; set; }
    
    public decimal? MinPrice { get; set; }
    
    public decimal? MaxPrice { get; set; }
    
    public List<string>? Tags { get; set; }
    
    public bool? InStock { get; set; }
    
    public int Page { get; set; } = 1;
    
    public int PageSize { get; set; } = 20;
    
    public string? SortBy { get; set; }
    
    public bool SortDescending { get; set; } = false;
}

public class ProductSearchResponse
{
    public List<ProductDto> Products { get; set; } = new();
    
    public int TotalCount { get; set; }
    
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalPages { get; set; }
} 