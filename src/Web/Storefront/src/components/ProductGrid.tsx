import { ProductCard } from './ProductCard';

// This would be replaced with actual API call
async function getProducts() {
  // TODO: Replace with actual API call to Product service
  return [
    {
      id: '1',
      name: 'Wireless Headphones',
      description: 'High-quality wireless headphones with noise cancellation',
      price: 99.99,
      category: 'Electronics',
      stockQuantity: 50,
      isActive: true,
      createdAt: new Date().toISOString(),
    },
    {
      id: '2',
      name: 'Smart Watch',
      description: 'Feature-rich smartwatch with health tracking',
      price: 199.99,
      category: 'Electronics',
      stockQuantity: 30,
      isActive: true,
      createdAt: new Date().toISOString(),
    },
    {
      id: '3',
      name: 'Running Shoes',
      description: 'Comfortable running shoes for all terrains',
      price: 79.99,
      category: 'Sports',
      stockQuantity: 100,
      isActive: true,
      createdAt: new Date().toISOString(),
    },
    {
      id: '4',
      name: 'Coffee Maker',
      description: 'Automatic coffee maker with programmable timer',
      price: 149.99,
      category: 'Home & Kitchen',
      stockQuantity: 25,
      isActive: true,
      createdAt: new Date().toISOString(),
    },
  ];
}

export async function ProductGrid() {
  const products = await getProducts();

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      {products.map((product) => (
        <ProductCard key={product.id} product={product} />
      ))}
    </div>
  );
} 