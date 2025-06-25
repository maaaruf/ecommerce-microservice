import { ProductDto, ProductSearchRequest, ProductSearchResponse, CreateProductRequest, UpdateProductRequest } from '@/types/product';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

class ProductService {
  private async request<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    const config: RequestInit = {
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
      ...options,
    };

    // Add auth token if available
    if (typeof window !== 'undefined') {
      const token = localStorage.getItem('accessToken');
      if (token) {
        config.headers = {
          ...config.headers,
          Authorization: `Bearer ${token}`,
        };
      }
    }

    const response = await fetch(url, config);
    
    if (!response.ok) {
      const error = await response.json().catch(() => ({ message: 'An error occurred' }));
      throw new Error(error.message || `HTTP error! status: ${response.status}`);
    }

    return response.json();
  }

  async getProducts(activeOnly: boolean = true): Promise<ProductDto[]> {
    return await this.request<ProductDto[]>(`/api/products?activeOnly=${activeOnly}`);
  }

  async getProductById(id: string): Promise<ProductDto> {
    return await this.request<ProductDto>(`/api/products/${id}`);
  }

  async searchProducts(request: ProductSearchRequest): Promise<ProductSearchResponse> {
    const params = new URLSearchParams();
    
    if (request.searchTerm) params.append('searchTerm', request.searchTerm);
    if (request.category) params.append('category', request.category);
    if (request.minPrice) params.append('minPrice', request.minPrice.toString());
    if (request.maxPrice) params.append('maxPrice', request.maxPrice.toString());
    if (request.inStock !== undefined) params.append('inStock', request.inStock.toString());
    if (request.page) params.append('page', request.page.toString());
    if (request.pageSize) params.append('pageSize', request.pageSize.toString());
    if (request.sortBy) params.append('sortBy', request.sortBy);
    if (request.sortDescending) params.append('sortDescending', request.sortDescending.toString());
    
    if (request.tags && request.tags.length > 0) {
      request.tags.forEach(tag => params.append('tags', tag));
    }

    return await this.request<ProductSearchResponse>(`/api/products/search?${params.toString()}`);
  }

  async getCategories(): Promise<string[]> {
    return await this.request<string[]>('/api/products/categories');
  }

  async getProductsByCategory(category: string): Promise<ProductDto[]> {
    return await this.request<ProductDto[]>(`/api/products/category/${encodeURIComponent(category)}`);
  }

  async createProduct(product: CreateProductRequest): Promise<ProductDto> {
    return await this.request<ProductDto>('/api/products', {
      method: 'POST',
      body: JSON.stringify(product),
    });
  }

  async updateProduct(id: string, product: UpdateProductRequest): Promise<ProductDto> {
    return await this.request<ProductDto>(`/api/products/${id}`, {
      method: 'PUT',
      body: JSON.stringify(product),
    });
  }

  async deleteProduct(id: string): Promise<void> {
    await this.request(`/api/products/${id}`, {
      method: 'DELETE',
    });
  }
}

export const productService = new ProductService(); 