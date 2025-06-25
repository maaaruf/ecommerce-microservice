import Link from 'next/link';

const categories = [
  {
    id: 'electronics',
    name: 'Electronics',
    description: 'Latest gadgets and technology',
    image: '/images/electronics.jpg',
  },
  {
    id: 'clothing',
    name: 'Clothing',
    description: 'Fashion and apparel',
    image: '/images/clothing.jpg',
  },
  {
    id: 'home',
    name: 'Home & Kitchen',
    description: 'Everything for your home',
    image: '/images/home.jpg',
  },
  {
    id: 'sports',
    name: 'Sports & Outdoors',
    description: 'Equipment and gear',
    image: '/images/sports.jpg',
  },
];

export function CategorySection() {
  return (
    <section className="py-16 bg-gray-50">
      <div className="container mx-auto px-4">
        <h2 className="text-3xl font-bold text-center mb-12">Shop by Category</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          {categories.map((category) => (
            <Link
              key={category.id}
              href={`/categories/${category.id}`}
              className="group"
            >
              <div className="card text-center hover:shadow-lg transition-shadow duration-300">
                {/* Category Image Placeholder */}
                <div className="aspect-square bg-gray-200 rounded-lg mb-4 flex items-center justify-center">
                  <div className="text-gray-500 text-sm">{category.name} Image</div>
                </div>
                
                <h3 className="text-xl font-semibold mb-2 group-hover:text-primary-600 transition-colors">
                  {category.name}
                </h3>
                <p className="text-gray-600 text-sm">
                  {category.description}
                </p>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </section>
  );
} 