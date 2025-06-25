import Link from 'next/link';

export function Hero() {
  return (
    <section className="bg-gradient-to-r from-primary-600 to-primary-700 text-white py-20">
      <div className="container mx-auto px-4 text-center">
        <h1 className="text-4xl md:text-6xl font-bold mb-6">
          Discover Amazing Products
        </h1>
        <p className="text-xl md:text-2xl mb-8 max-w-2xl mx-auto">
          Shop the latest trends with confidence. Quality products, competitive prices, and excellent service.
        </p>
        <div className="flex flex-col sm:flex-row gap-4 justify-center">
          <Link
            href="/products"
            className="bg-white text-primary-600 px-8 py-3 rounded-lg font-semibold hover:bg-gray-100 transition-colors"
          >
            Shop Now
          </Link>
          <Link
            href="/categories"
            className="border-2 border-white text-white px-8 py-3 rounded-lg font-semibold hover:bg-white hover:text-primary-600 transition-colors"
          >
            Browse Categories
          </Link>
        </div>
      </div>
    </section>
  );
} 