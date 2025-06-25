import { Suspense } from 'react';
import { Metadata } from 'next';
import { ProductGrid } from '@/components/ProductGrid';
import { Hero } from '@/components/Hero';
import { CategorySection } from '@/components/CategorySection';
import { LoadingSpinner } from '@/components/LoadingSpinner';

export const metadata: Metadata = {
  title: 'Home',
  description: 'Welcome to our e-commerce store. Discover amazing products at great prices.',
  openGraph: {
    title: 'E-commerce Store - Your Online Shopping Destination',
    description: 'Welcome to our e-commerce store. Discover amazing products at great prices.',
  },
};

export default function HomePage() {
  return (
    <main className="min-h-screen">
      <Hero />
      
      <section className="container mx-auto px-4 py-8">
        <h2 className="text-3xl font-bold text-center mb-8">Featured Products</h2>
        <Suspense fallback={<LoadingSpinner />}>
          <ProductGrid />
        </Suspense>
      </section>
      
      <CategorySection />
      
      <section className="bg-gray-100 py-16">
        <div className="container mx-auto px-4 text-center">
          <h2 className="text-3xl font-bold mb-4">Why Choose Us?</h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mt-8">
            <div className="card text-center">
              <h3 className="text-xl font-semibold mb-2">Fast Delivery</h3>
              <p className="text-gray-600">Get your orders delivered quickly and safely</p>
            </div>
            <div className="card text-center">
              <h3 className="text-xl font-semibold mb-2">Quality Products</h3>
              <p className="text-gray-600">We only sell high-quality products</p>
            </div>
            <div className="card text-center">
              <h3 className="text-xl font-semibold mb-2">24/7 Support</h3>
              <p className="text-gray-600">Our customer support is always here to help</p>
            </div>
          </div>
        </div>
      </section>
    </main>
  );
} 