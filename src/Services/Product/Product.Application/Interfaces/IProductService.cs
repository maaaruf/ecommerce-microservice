using Shared.Contracts.Models;

namespace Product.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(string id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
    Task<ProductSearchResponse> SearchProductsAsync(ProductSearchRequest request);
    Task<ProductDto> CreateProductAsync(CreateProductRequest request, string createdBy);
    Task<ProductDto> UpdateProductAsync(string id, UpdateProductRequest request, string updatedBy);
    Task DeleteProductAsync(string id);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
    Task UpdateStockAsync(string productId, int quantity);
} 