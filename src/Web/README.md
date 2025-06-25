# E-commerce Frontend Architecture

This directory contains two separate frontend applications designed for different purposes:

## 🛍️ Storefront (Next.js) - SEO Friendly

**Location:** `src/Web/Storefront/`
**Port:** 3000
**Purpose:** Customer-facing e-commerce store

### Features:
- **Server-Side Rendering (SSR)** for better SEO
- **Static Generation (SSG)** for product pages
- **SEO Optimized** with meta tags, Open Graph, and structured data
- **Fast Loading** with optimized images and code splitting
- **Mobile Responsive** design
- **Search Engine Friendly** URLs and content

### Key Components:
- Product catalog with search and filtering
- Product detail pages with reviews
- Shopping cart functionality
- User authentication and registration
- Order tracking
- Category browsing

### Technologies:
- Next.js 14 with App Router
- TypeScript
- Tailwind CSS
- Redux Toolkit for state management
- React Hook Form for form handling

## 🏢 Admin Panel (React) - SPA

**Location:** `src/Web/AdminPanel/`
**Port:** 3001
**Purpose:** Administrative interface for store management

### Features:
- **Single Page Application (SPA)** for rich interactions
- **Real-time Dashboard** with analytics
- **Product Management** (CRUD operations)
- **Order Management** with status updates
- **User Management** and role-based access
- **Inventory Management** with stock alerts

### Key Components:
- Dashboard with analytics and metrics
- Product management interface
- Order processing and tracking
- User management and permissions
- Sales reports and analytics

### Technologies:
- React 18 with Vite
- TypeScript
- Tailwind CSS
- Redux Toolkit for state management
- React Router for navigation
- Recharts for data visualization

## 🚀 Getting Started

### Development

1. **Storefront (Next.js):**
   ```bash
   cd src/Web/Storefront
   npm install
   npm run dev
   ```
   Access at: http://localhost:3000

2. **Admin Panel (React):**
   ```bash
   cd src/Web/AdminPanel
   npm install
   npm run dev
   ```
   Access at: http://localhost:3001

### Production Build

1. **Storefront:**
   ```bash
   cd src/Web/Storefront
   npm run build
   npm start
   ```

2. **Admin Panel:**
   ```bash
   cd src/Web/AdminPanel
   npm run build
   npm run preview
   ```

### Docker Deployment

Both applications are configured for Docker deployment:

```bash
# Build and run all services
docker-compose up --build

# Storefront: http://localhost:3000
# Admin Panel: http://localhost:3001
```

## 🔧 Configuration

### Environment Variables

**Storefront:**
- `NEXT_PUBLIC_API_URL`: API Gateway URL

**Admin Panel:**
- `VITE_API_URL`: API Gateway URL

### API Integration

Both frontends communicate with the backend through the API Gateway:
- **Storefront:** Uses Next.js API routes and server-side data fetching
- **Admin Panel:** Uses Vite proxy configuration for development

## 📱 Responsive Design

Both applications are fully responsive and optimized for:
- Desktop (1024px+)
- Tablet (768px - 1023px)
- Mobile (320px - 767px)

## 🔒 Security

- CORS configuration for cross-origin requests
- Security headers for XSS protection
- Input validation and sanitization
- JWT token-based authentication

## 📊 Performance

- **Storefront:** Optimized for Core Web Vitals and SEO
- **Admin Panel:** Optimized for fast interactions and real-time updates
- Image optimization and lazy loading
- Code splitting and tree shaking
- Caching strategies for static assets

## 🧪 Testing

Both applications include:
- Unit tests with Jest
- Component tests with React Testing Library
- E2E tests with Playwright (planned)

## 📈 Monitoring

- Error tracking and logging
- Performance monitoring
- User analytics
- SEO tracking for storefront 