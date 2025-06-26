using AutoMapper;
using Product.Application.Interfaces;
using Product.Domain.Entities;
using Product.Domain.Interfaces;
using Shared.Contracts.Models;

namespace Product.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto> GetProductByIdAsync(string id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {id} not found");

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
    {
        var products = await _productRepository.GetActiveProductsAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductSearchResponse> SearchProductsAsync(ProductSearchRequest request)
    {
        var products = await _productRepository.SearchProductsAsync(request);
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

        // TODO: Implement proper pagination and total count
        return new ProductSearchResponse
        {
            Products = productDtos.ToList(),
            TotalCount = productDtos.Count(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)productDtos.Count() / request.PageSize)
        };
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request, string createdBy)
    {
        var product = new Product.Domain.Entities.Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category,
            Tags = request.Tags,
            ImageUrls = request.ImageUrls,
            CreatedBy = createdBy,
            UpdatedBy = createdBy
        };

        var createdProduct = await _productRepository.CreateAsync(product);
        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<ProductDto> UpdateProductAsync(string id, UpdateProductRequest request, string updatedBy)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
            throw new InvalidOperationException($"Product with ID {id} not found");

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.Name))
            existingProduct.Name = request.Name;
        
        if (!string.IsNullOrEmpty(request.Description))
            existingProduct.Description = request.Description;
        
        if (request.Price.HasValue)
            existingProduct.Price = request.Price.Value;
        
        if (request.StockQuantity.HasValue)
            existingProduct.UpdateStock(request.StockQuantity.Value);
        
        if (!string.IsNullOrEmpty(request.Category))
            existingProduct.Category = request.Category;
        
        if (request.Tags != null)
            existingProduct.Tags = request.Tags;
        
        if (request.ImageUrls != null)
            existingProduct.ImageUrls = request.ImageUrls;
        
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                existingProduct.Activate();
            else
                existingProduct.Deactivate();
        }

        existingProduct.UpdatedBy = updatedBy;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task DeleteProductAsync(string id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {id} not found");

        await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _productRepository.GetCategoriesAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
    {
        var products = await _productRepository.GetProductsByCategoryAsync(category);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task UpdateStockAsync(string productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {productId} not found");

        product.UpdateStock(quantity);
        await _productRepository.UpdateStockAsync(productId, quantity);
    }
} 