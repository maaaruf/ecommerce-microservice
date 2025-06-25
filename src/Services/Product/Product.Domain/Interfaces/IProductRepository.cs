using Product.Domain.Entities;
using Shared.Contracts.Models;

namespace Product.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    Task<IEnumerable<Product>> SearchProductsAsync(ProductSearchRequest request);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
    Task UpdateStockAsync(string productId, int quantity);
} 