import React, { useState } from 'react';
import { Header } from './components/Header';
import { Login } from './components/Login';
import { Register } from './components/Register';

interface User {
  username: string;
  email: string;
}

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState<User | undefined>();
  const [showLogin, setShowLogin] = useState(false);
  const [showRegister, setShowRegister] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const handleLogin = async (username: string, password: string) => {
    setIsLoading(true);
    setError(undefined);
    
    try {
      // TODO: Implement actual API call
      console.log('Logging in with:', username, password);
      
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Mock successful login
      setUser({ username, email: `${username}@example.com` });
      setIsAuthenticated(true);
      setShowLogin(false);
    } catch (err) {
      setError('Login failed. Please check your credentials.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleRegister = async (userData: any) => {
    setIsLoading(true);
    setError(undefined);
    
    try {
      // TODO: Implement actual API call
      console.log('Registering user:', userData);
      
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Mock successful registration
      setUser({ username: userData.username, email: userData.email });
      setIsAuthenticated(true);
      setShowRegister(false);
    } catch (err) {
      setError('Registration failed. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleLogout = () => {
    setUser(undefined);
    setIsAuthenticated(false);
  };

  const handleShowLogin = () => {
    setShowLogin(true);
    setShowRegister(false);
    setError(undefined);
  };

  const handleShowRegister = () => {
    setShowRegister(true);
    setShowLogin(false);
    setError(undefined);
  };

  // Show login/register forms if not authenticated
  if (showLogin) {
    return (
      <Login
        onLogin={handleLogin}
        onRegister={handleShowRegister}
        isLoading={isLoading}
        error={error}
      />
    );
  }

  if (showRegister) {
    return (
      <Register
        onRegister={handleRegister}
        onLogin={handleShowLogin}
        isLoading={isLoading}
        error={error}
      />
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Header
        isAuthenticated={isAuthenticated}
        user={user}
        onLogin={handleShowLogin}
        onLogout={handleLogout}
      />
      <main className="container mx-auto px-4 py-8">
        <div className="text-center">
          <h2 className="text-4xl font-bold text-gray-900 mb-4">
            Welcome to E-commerce Microservices
          </h2>
          <p className="text-lg text-gray-600 mb-8">
            This is a comprehensive e-commerce application built with microservices architecture.
          </p>
          
          {!isAuthenticated ? (
            <div className="space-y-4">
              <p className="text-gray-600">Please log in to access all features.</p>
              <button
                onClick={handleShowLogin}
                className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors"
              >
                Get Started
              </button>
            </div>
          ) : (
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
          )}
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