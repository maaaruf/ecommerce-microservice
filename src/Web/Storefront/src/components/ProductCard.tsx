'use client';

import Link from 'next/link';
import { ShoppingCart, Heart } from 'lucide-react';

interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  category: string;
  stockQuantity: number;
  isActive: boolean;
  createdAt: string;
}

interface ProductCardProps {
  product: Product;
}

export function ProductCard({ product }: ProductCardProps) {
  const handleAddToCart = (e: React.MouseEvent) => {
    e.preventDefault();
    // TODO: Implement add to cart functionality
    console.log('Adding to cart:', product.id);
  };

  const handleAddToWishlist = (e: React.MouseEvent) => {
    e.preventDefault();
    // TODO: Implement add to wishlist functionality
    console.log('Adding to wishlist:', product.id);
  };

  return (
    <Link href={`/products/${product.id}`} className="group">
      <div className="card hover:shadow-lg transition-shadow duration-300 h-full flex flex-col">
        {/* Product Image Placeholder */}
        <div className="aspect-square bg-gray-200 rounded-t-lg mb-4 flex items-center justify-center">
          <div className="text-gray-500 text-sm">Product Image</div>
        </div>
        
        {/* Product Info */}
        <div className="flex-1 flex flex-col">
          <h3 className="font-semibold text-lg mb-2 group-hover:text-primary-600 transition-colors">
            {product.name}
          </h3>
          <p className="text-gray-600 text-sm mb-4 line-clamp-2">
            {product.description}
          </p>
          
          <div className="mt-auto">
            <div className="flex items-center justify-between mb-3">
              <span className="text-2xl font-bold text-primary-600">
                ${product.price.toFixed(2)}
              </span>
              <span className="text-sm text-gray-500">
                {product.stockQuantity > 0 ? `${product.stockQuantity} in stock` : 'Out of stock'}
              </span>
            </div>
            
            <div className="flex gap-2">
              <button
                onClick={handleAddToCart}
                disabled={product.stockQuantity === 0}
                className="flex-1 bg-primary-600 text-white py-2 px-4 rounded-md hover:bg-primary-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
              >
                <ShoppingCart className="h-4 w-4" />
                Add to Cart
              </button>
              <button
                onClick={handleAddToWishlist}
                className="p-2 border border-gray-300 rounded-md hover:bg-gray-50 transition-colors"
              >
                <Heart className="h-4 w-4" />
              </button>
            </div>
          </div>
        </div>
      </div>
    </Link>
  );
} 