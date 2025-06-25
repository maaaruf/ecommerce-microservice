import React from 'react';

interface HeaderProps {
  isAuthenticated: boolean;
  user?: {
    username: string;
    email: string;
  };
  onLogin: () => void;
  onLogout: () => void;
}

export const Header: React.FC<HeaderProps> = ({ isAuthenticated, user, onLogin, onLogout }) => {
  return (
    <header className="bg-white shadow-lg">
      <div className="container mx-auto px-4 py-4">
        <div className="flex justify-between items-center">
          <div className="flex items-center space-x-4">
            <h1 className="text-2xl font-bold text-gray-900">E-commerce App</h1>
            <nav className="hidden md:flex space-x-6">
              <a href="/" className="text-gray-600 hover:text-gray-900">Home</a>
              <a href="/products" className="text-gray-600 hover:text-gray-900">Products</a>
              <a href="/cart" className="text-gray-600 hover:text-gray-900">Cart</a>
              {isAuthenticated && (
                <a href="/orders" className="text-gray-600 hover:text-gray-900">Orders</a>
              )}
            </nav>
          </div>
          
          <div className="flex items-center space-x-4">
            {isAuthenticated ? (
              <div className="flex items-center space-x-4">
                <div className="text-sm text-gray-600">
                  Welcome, {user?.username || 'User'}!
                </div>
                <button
                  onClick={onLogout}
                  className="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 transition-colors"
                >
                  Logout
                </button>
              </div>
            ) : (
              <button
                onClick={onLogin}
                className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition-colors"
              >
                Login
              </button>
            )}
          </div>
        </div>
      </div>
    </header>
  );
}; 