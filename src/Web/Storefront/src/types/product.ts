export interface ProductDto {
  id: string;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  category: string;
  tags: string[];
  imageUrls: string[];
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateProductRequest {
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  category: string;
  tags: string[];
  imageUrls: string[];
}

export interface UpdateProductRequest {
  name?: string;
  description?: string;
  price?: number;
  stockQuantity?: number;
  category?: string;
  tags?: string[];
  imageUrls?: string[];
  isActive?: boolean;
}

export interface ProductSearchRequest {
  searchTerm?: string;
  category?: string;
  minPrice?: number;
  maxPrice?: number;
  tags?: string[];
  inStock?: boolean;
  page: number;
  pageSize: number;
  sortBy?: string;
  sortDescending: boolean;
}

export interface ProductSearchResponse {
  products: ProductDto[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface ProductState {
  products: ProductDto[];
  currentProduct: ProductDto | null;
  categories: string[];
  loading: boolean;
  error: string | null;
  searchResults: ProductSearchResponse | null;
} 