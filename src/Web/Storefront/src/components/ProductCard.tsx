'use client';

import Link from 'next/link';
import { ShoppingCart, Heart, Star } from 'lucide-react';
import { ProductDto } from '@/types/product';

interface ProductCardProps {
  product: ProductDto;
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

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(price);
  };

  const isInStock = product.stockQuantity > 0 && product.isActive;

  return (
    <Link href={`/products/${product.id}`} className="group">
      <div className="bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 overflow-hidden">
        {/* Product Image */}
        <div className="relative aspect-square bg-gray-200 overflow-hidden">
          {product.imageUrls && product.imageUrls.length > 0 ? (
            <img
              src={product.imageUrls[0]}
              alt={product.name}
              className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-gray-400">
              <span>No Image</span>
            </div>
          )}
          
          {/* Wishlist Button */}
          <button
            onClick={handleAddToWishlist}
            className="absolute top-2 right-2 p-2 bg-white rounded-full shadow-md hover:bg-gray-50 transition-colors"
          >
            <Heart className="h-4 w-4 text-gray-600" />
          </button>

          {/* Stock Status */}
          {!isInStock && (
            <div className="absolute top-2 left-2 bg-red-500 text-white px-2 py-1 rounded text-xs font-medium">
              Out of Stock
            </div>
          )}
        </div>

        {/* Product Info */}
        <div className="p-4">
          {/* Category */}
          <div className="text-xs text-gray-500 uppercase tracking-wide mb-1">
            {product.category}
          </div>

          {/* Product Name */}
          <h3 className="font-semibold text-gray-900 mb-2 line-clamp-2 group-hover:text-primary-600 transition-colors">
            {product.name}
          </h3>

          {/* Description */}
          <p className="text-sm text-gray-600 mb-3 line-clamp-2">
            {product.description}
          </p>

          {/* Price */}
          <div className="flex items-center justify-between mb-3">
            <span className="text-lg font-bold text-gray-900">
              {formatPrice(product.price)}
            </span>
            
            {/* Rating */}
            <div className="flex items-center text-sm text-gray-500">
              <Star className="h-4 w-4 fill-yellow-400 text-yellow-400" />
              <span className="ml-1">4.5</span>
              <span className="ml-1">(24)</span>
            </div>
          </div>

          {/* Stock Info */}
          <div className="text-sm text-gray-500 mb-3">
            {isInStock ? (
              <span className="text-green-600">
                {product.stockQuantity} in stock
              </span>
            ) : (
              <span className="text-red-600">Out of stock</span>
            )}
          </div>

          {/* Add to Cart Button */}
          <button
            onClick={handleAddToCart}
            disabled={!isInStock}
            className="w-full bg-primary-600 text-white py-2 px-4 rounded-md hover:bg-primary-700 transition-colors disabled:bg-gray-300 disabled:cursor-not-allowed flex items-center justify-center gap-2"
          >
            <ShoppingCart className="h-4 w-4" />
            {isInStock ? 'Add to Cart' : 'Out of Stock'}
          </button>
        </div>
      </div>
    </Link>
  );
} 