import type { Metadata } from 'next';
import { Inter } from 'next/font/google';
import './globals.css';

const inter = Inter({ subsets: ['latin'] });

export const metadata: Metadata = {
  title: {
    default: 'E-commerce Store | Your Online Shopping Destination',
    template: '%s | E-commerce Store'
  },
  description: 'Discover amazing products at great prices. Shop online with confidence.',
  keywords: ['e-commerce', 'online shopping', 'products', 'retail'],
  authors: [{ name: 'E-commerce Team' }],
  creator: 'E-commerce Store',
  publisher: 'E-commerce Store',
  formatDetection: {
    email: false,
    address: false,
    telephone: false,
  },
  metadataBase: new URL('http://localhost:3000'),
  alternates: {
    canonical: '/',
  },
  openGraph: {
    title: 'E-commerce Store | Your Online Shopping Destination',
    description: 'Discover amazing products at great prices. Shop online with confidence.',
    url: 'http://localhost:3000',
    siteName: 'E-commerce Store',
    images: [
      {
        url: '/og-image.jpg',
        width: 1200,
        height: 630,
        alt: 'E-commerce Store',
      },
    ],
    locale: 'en_US',
    type: 'website',
  },
  twitter: {
    card: 'summary_large_image',
    title: 'E-commerce Store | Your Online Shopping Destination',
    description: 'Discover amazing products at great prices. Shop online with confidence.',
    images: ['/og-image.jpg'],
  },
  robots: {
    index: true,
    follow: true,
    googleBot: {
      index: true,
      follow: true,
      'max-video-preview': -1,
      'max-image-preview': 'large',
      'max-snippet': -1,
    },
  },
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className={inter.className}>
        {children}
      </body>
    </html>
  );
} 