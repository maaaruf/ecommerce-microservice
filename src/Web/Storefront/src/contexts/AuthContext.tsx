'use client';

import React, { createContext, useContext, useEffect, useState } from 'react';
import { authService } from '@/services/authService';
import { UserDto, AuthState } from '@/types/auth';

interface AuthContextType extends AuthState {
  login: (username: string, password: string) => Promise<void>;
  register: (userData: any) => Promise<void>;
  logout: () => Promise<void>;
  clearError: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [state, setState] = useState<AuthState>({
    user: null,
    isAuthenticated: false,
    loading: true,
    error: null,
  });

  useEffect(() => {
    // Check if user is already authenticated on app load
    const checkAuth = async () => {
      try {
        if (authService.isAuthenticated()) {
          const user = authService.getCurrentUser();
          if (user) {
            setState(prev => ({
              ...prev,
              user,
              isAuthenticated: true,
              loading: false,
            }));
          } else {
            // Try to get fresh user data
            const profile = await authService.getProfile();
            setState(prev => ({
              ...prev,
              user: profile,
              isAuthenticated: true,
              loading: false,
            }));
          }
        } else {
          setState(prev => ({ ...prev, loading: false }));
        }
      } catch (error) {
        console.error('Auth check failed:', error);
        setState(prev => ({ ...prev, loading: false }));
      }
    };

    checkAuth();
  }, []);

  const login = async (username: string, password: string) => {
    setState(prev => ({ ...prev, loading: true, error: null }));
    
    try {
      const response = await authService.login({ username, password });
      setState(prev => ({
        ...prev,
        user: response.user,
        isAuthenticated: true,
        loading: false,
        error: null,
      }));
    } catch (error) {
      setState(prev => ({
        ...prev,
        loading: false,
        error: error instanceof Error ? error.message : 'Login failed',
      }));
      throw error;
    }
  };

  const register = async (userData: any) => {
    setState(prev => ({ ...prev, loading: true, error: null }));
    
    try {
      await authService.register(userData);
      setState(prev => ({ ...prev, loading: false, error: null }));
    } catch (error) {
      setState(prev => ({
        ...prev,
        loading: false,
        error: error instanceof Error ? error.message : 'Registration failed',
      }));
      throw error;
    }
  };

  const logout = async () => {
    setState(prev => ({ ...prev, loading: true }));
    
    try {
      await authService.logout();
      setState({
        user: null,
        isAuthenticated: false,
        loading: false,
        error: null,
      });
    } catch (error) {
      console.error('Logout error:', error);
      // Still clear state even if API call fails
      setState({
        user: null,
        isAuthenticated: false,
        loading: false,
        error: null,
      });
    }
  };

  const clearError = () => {
    setState(prev => ({ ...prev, error: null }));
  };

  const value: AuthContextType = {
    ...state,
    login,
    register,
    logout,
    clearError,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
} 