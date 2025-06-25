import React from 'react';

function App() {
  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white shadow">
        <div className="container mx-auto px-4 py-6">
          <h1 className="text-2xl font-bold text-gray-900">E-commerce App</h1>
        </div>
      </header>
      <main className="container mx-auto px-4 py-8">
        <div className="text-center">
          <h2 className="text-4xl font-bold text-gray-900 mb-4">
            Welcome to E-commerce Microservices
          </h2>
          <p className="text-lg text-gray-600">
            This is a comprehensive e-commerce application built with microservices architecture.
          </p>
          <div className="mt-8">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="bg-white p-6 rounded-lg shadow">
                <h3 className="text-xl font-semibold mb-2">Products</h3>
                <p className="text-gray-600">Browse and search products</p>
              </div>
              <div className="bg-white p-6 rounded-lg shadow">
                <h3 className="text-xl font-semibold mb-2">Cart</h3>
                <p className="text-gray-600">Manage your shopping cart</p>
              </div>
              <div className="bg-white p-6 rounded-lg shadow">
                <h3 className="text-xl font-semibold mb-2">Orders</h3>
                <p className="text-gray-600">Track your orders</p>
              </div>
            </div>
          </div>
        </div>
      </main>
      <footer className="bg-gray-800 text-white py-8">
        <div className="container mx-auto px-4 text-center">
          <p>&copy; 2024 E-commerce Microservices. All rights reserved.</p>
        </div>
      </footer>
    </div>
  );
}

export default App; 