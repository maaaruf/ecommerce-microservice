using Product.Domain.Entities;
using Shared.Contracts.Models;

namespace Product.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product.Domain.Entities.Product?> GetByIdAsync(string id);
    Task<IEnumerable<Product.Domain.Entities.Product>> GetAllAsync();
    Task<IEnumerable<Product.Domain.Entities.Product>> GetActiveProductsAsync();
    Task<IEnumerable<Product.Domain.Entities.Product>> SearchProductsAsync(ProductSearchRequest request);
    Task<Product.Domain.Entities.Product> CreateAsync(Product.Domain.Entities.Product product);
    Task<Product.Domain.Entities.Product> UpdateAsync(Product.Domain.Entities.Product product);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<IEnumerable<Product.Domain.Entities.Product>> GetProductsByCategoryAsync(string category);
    Task UpdateStockAsync(string productId, int quantity);
} 