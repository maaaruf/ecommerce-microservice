# 🛒 E-commerce Microservices Platform

A modern, scalable e-commerce platform built with **.NET 8 microservices**, **Next.js storefront**, **React admin panel**, and **cloud-native technologies**. This platform demonstrates best practices in microservices architecture, event-driven design, and modern web development.

## 🏗️ Architecture Overview

### Backend Services
| Service | Purpose | Technology Stack |
|---------|---------|------------------|
| **Auth Service** | User authentication and authorization | .NET 8, PostgreSQL, Keycloak OAuth2/OIDC |
| **Product Service** | Product catalog and inventory management | .NET 8, PostgreSQL, MongoDB, Elasticsearch |
| **Cart Service** | Shopping cart management | .NET 8, Redis, PostgreSQL |
| **Order Service** | Order lifecycle management | .NET 8, PostgreSQL, RabbitMQ |
| **Payment Service** | Payment processing | .NET 8, PostgreSQL, Stripe, RabbitMQ |
| **Notification Service** | Email, SMS, and webhook notifications | .NET 8, PostgreSQL, SendGrid, Twilio |
| **API Gateway** | Service aggregation and routing | .NET 8, YARP |

### Frontend Applications
| Application | Purpose | Technology Stack |
|-------------|---------|------------------|
| **Storefront** | Customer-facing e-commerce store | Next.js 14, TypeScript, TailwindCSS, Redux Toolkit |
| **Admin Panel** | Administrative interface | React 18, Vite, TypeScript, TailwindCSS, Redux Toolkit |

### Infrastructure
- **Database**: PostgreSQL (per service), Redis (caching/pub-sub), MongoDB (catalog)
- **Message Broker**: RabbitMQ for async communication
- **Identity**: Keycloak OAuth2/OIDC
- **Containerization**: Docker + Docker Compose
- **Orchestration**: Kubernetes (optional)
- **CI/CD**: GitHub Actions
- **Monitoring**: ELK Stack (Elasticsearch, Logstash, Kibana)

## 🚀 Quick Start

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Node.js 18+](https://nodejs.org/)
- [Git](https://git-scm.com/)

### Option 1: Docker Compose (Recommended)

```bash
# Clone the repository
git clone https://github.com/maaaruf/ecommerce-microservice.git
cd ecommerce-microservice

# Start all services
docker-compose up -d

# Wait for Keycloak to be ready, then configure it
# Choose your operating system:

# Windows (PowerShell):
.\scripts\setup-keycloak.ps1

# Windows (Batch file - easiest):
scripts\setup-keycloak.bat

# Windows (Git Bash/WSL):
bash scripts/setup-keycloak.sh

# Linux/Mac:
chmod +x scripts/setup-keycloak.sh
./scripts/setup-keycloak.sh

# Check service status
docker-compose ps
```

**Access Points:**
- 🛍️ **Storefront**: http://localhost:3000
- 🏢 **Admin Panel**: http://localhost:3001
- 🔌 **API Gateway**: http://localhost:5000
- 🔐 **Keycloak Admin**: http://localhost:8080 (admin/admin)
- 📊 **Kibana**: http://localhost:5601
- 🐰 **RabbitMQ Management**: http://localhost:15672

### Option 2: Local Development

```bash
# Start infrastructure services only
docker-compose up -d postgres-auth postgres-product postgres-cart postgres-order postgres-payment postgres-notification mongodb redis rabbitmq keycloak elasticsearch kibana

# Configure Keycloak (choose your OS):
# Windows: .\scripts\setup-keycloak.ps1
# Linux/Mac: ./scripts/setup-keycloak.sh

# Start backend services
dotnet run --project src/Services/Auth/Auth.API
dotnet run --project src/Services/Product/Product.API
dotnet run --project src/Services/Cart/Cart.API
dotnet run --project src/Services/Order/Order.API
dotnet run --project src/Services/Payment/Payment.API
dotnet run --project src/Services/Notification/Notification.API
dotnet run --project src/Gateway/API.Gateway

# Start frontend applications
# Storefront (Next.js)
cd src/Web/Storefront
npm install
npm run dev

# Admin Panel (React)
cd src/Web/AdminPanel
npm install
npm run dev
```

## 🔐 Authentication with Keycloak

The platform uses **Keycloak** for enterprise-grade OAuth2/OIDC authentication:

### Keycloak Features
- ✅ **OAuth2/OIDC Compliance**: Standard authentication protocols
- ✅ **User Management**: Centralized user administration
- ✅ **Role-Based Access Control**: Fine-grained permissions
- ✅ **Multi-Factor Authentication**: Built-in 2FA support
- ✅ **Social Login**: Integration with Google, Facebook, etc.
- ✅ **Single Sign-On**: Seamless authentication across services

### Test Credentials
After running the setup script, you can use these test credentials:
- **Username**: `testuser`
- **Password**: `Test123!`
- **Email**: `test@example.com`

### Authentication Flow
1. **Login**: Redirect to Keycloak login page
2. **Authorization**: User authenticates with Keycloak
3. **Callback**: Auth service handles OAuth2 callback
4. **Token Management**: JWT tokens for API access
5. **User Sync**: User data synchronized with local database

For detailed setup instructions, see [Keycloak Integration Guide](docs/keycloak-integration.md).

## 📁 Project Structure

```
├── src/
│   ├── Services/                    # Microservices
│   │   ├── Auth/                   # Authentication service (Keycloak integration)
│   │   │   ├── Auth.API/          # Web API layer
│   │   │   ├── Auth.Application/  # Business logic layer
│   │   │   ├── Auth.Domain/       # Domain entities
│   │   │   └── Auth.Infrastructure/ # Data access layer
│   │   ├── Product/               # Product catalog service
│   │   ├── Cart/                  # Shopping cart service
│   │   ├── Order/                 # Order management service
│   │   ├── Payment/               # Payment processing service
│   │   └── Notification/          # Notification service
│   ├── Gateway/                   # API Gateway (YARP)
│   ├── Shared/                    # Shared libraries
│   │   └── Shared.Contracts/      # DTOs and events
│   └── Web/                       # Frontend applications
│       ├── Storefront/            # Next.js customer storefront
│       │   ├── src/
│       │   │   ├── app/           # Next.js App Router
│       │   │   └── components/    # React components
│       │   └── public/            # Static assets
│       └── AdminPanel/            # React admin interface
│           ├── src/
│           │   ├── components/    # React components
│           │   └── store/         # Redux store
│           └── public/            # Static assets
├── docs/                          # Documentation
├── infrastructure/                # Infrastructure configs
├── scripts/                       # Build and deployment scripts
│   ├── setup-keycloak.sh         # Keycloak configuration (Linux/Mac)
│   ├── setup-keycloak.ps1        # Keycloak configuration (Windows PowerShell)
│   └── setup-keycloak.bat        # Keycloak configuration (Windows Batch)
└── docker-compose.yml            # Docker services configuration
```

## 🛍️ Frontend Architecture

### Storefront (Next.js) - SEO Friendly
**Purpose**: Customer-facing e-commerce store with excellent SEO performance

**Key Features:**
- **Server-Side Rendering (SSR)** for better SEO
- **Static Generation (SSG)** for product pages
- **SEO Optimized** with meta tags, Open Graph, and structured data
- **Fast Loading** with optimized images and code splitting
- **Mobile Responsive** design
- **Search Engine Friendly** URLs and content
- **OAuth2 Integration** with Keycloak for authentication

**Technologies:**
- Next.js 14 with App Router
- TypeScript
- TailwindCSS
- Redux Toolkit for state management
- React Hook Form for form handling

### Admin Panel (React) - SPA
**Purpose**: Administrative interface for store management

**Key Features:**
- **Single Page Application (SPA)** for rich interactions
- **Real-time Dashboard** with analytics
- **Product Management** (CRUD operations)
- **Order Management** with status updates
- **User Management** and role-based access
- **Inventory Management** with stock alerts
- **OAuth2 Integration** with Keycloak for authentication

**Technologies:**
- React 18 with Vite
- TypeScript
- TailwindCSS
- Redux Toolkit for state management
- React Router for navigation
- Recharts for data visualization

## 🏛️ Architecture Principles

### Clean Architecture
- **Domain Layer**: Core business logic and entities
- **Application Layer**: Use cases and business rules
- **Infrastructure Layer**: External concerns (databases, APIs)
- **API Layer**: Controllers and HTTP concerns

### Domain-Driven Design (DDD)
- **Bounded Contexts**: Each service represents a distinct business domain
- **Aggregates**: Business entities with clear boundaries
- **Domain Events**: Events that represent business occurrences
- **Value Objects**: Immutable objects representing domain concepts

### Event-Driven Architecture
- **Integration Events**: Cross-service communication
- **Event Sourcing**: Audit trail of all changes
- **CQRS**: Separate read and write models

## 🔧 Key Features

### Authentication & Authorization
- **Keycloak OAuth2/OIDC**: Enterprise-grade identity management
- **Role-based access control (RBAC)**: Fine-grained permissions
- **Token-based authentication**: JWT tokens for API access
- **Single Sign-On (SSO)**: Seamless authentication across services

### API Gateway
- Request routing and aggregation
- Authentication and authorization
- Rate limiting and throttling
- Request/response transformation

### Event-Driven Communication
- RabbitMQ message broker
- Integration events for cross-service communication
- Event sourcing for audit trails
- Saga pattern for distributed transactions

### Monitoring & Observability
- Centralized logging with ELK Stack
- Health checks for all services
- Distributed tracing
- Performance metrics

### Security
- HTTPS enforcement
- Input validation and sanitization
- SQL injection prevention
- XSS protection
- OAuth2/OIDC security standards

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run specific service tests
dotnet test src/Services/Auth/Auth.Tests
dotnet test src/Services/Product/Product.Tests

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Frontend tests
cd src/Web/Storefront && npm test
cd src/Web/AdminPanel && npm test
```

## 📚 Documentation

- [🏗️ Architecture Guide](docs/architecture.md) - Detailed architecture and design decisions
- [🛠️ Development Guide](docs/development.md) - Setup, workflow, and best practices
- [🚀 Deployment Guide](docs/deployment.md) - Production deployment with Docker and Kubernetes
- [🔐 Keycloak Integration Guide](docs/keycloak-integration.md) - Keycloak setup and usage
- [🛍️ Frontend Guide](src/Web/README.md) - Frontend architecture and development
- [🤝 Contributing Guide](CONTRIBUTING.md) - Guidelines for contributors

## 🔄 Development Workflow

### Adding New Features
1. **Domain Layer**: Define entities, value objects, and domain services
2. **Application Layer**: Create use cases and application services
3. **Infrastructure Layer**: Implement repositories and external integrations
4. **API Layer**: Add controllers and endpoints
5. **Frontend**: Update storefront and/or admin panel
6. **Tests**: Write unit and integration tests

### Code Quality
- Follow **SOLID principles**
- Use **Domain-Driven Design** patterns
- Implement **CQRS** with MediatR
- Write **clean, readable code**
- Maintain **80%+ test coverage**

## 🚀 Deployment

### Docker Deployment
```bash
# Build and run with Docker Compose
docker-compose up -d

# Configure Keycloak (choose your OS):
# Windows: .\scripts\setup-keycloak.ps1
# Linux/Mac: ./scripts/setup-keycloak.sh

# Production deployment
docker-compose -f docker-compose.prod.yml up -d
```

### Kubernetes Deployment
```bash
# Apply Kubernetes manifests
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/config/
kubectl apply -f k8s/services/
kubectl apply -f k8s/gateway/
kubectl apply -f k8s/storefront/
kubectl apply -f k8s/admin-panel/
```

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details on:

- Code of conduct
- Development setup
- Coding standards
- Testing requirements
- Pull request process

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [.NET 8](https://dotnet.microsoft.com/) - Modern, fast, and cross-platform framework
- [Next.js](https://nextjs.org/) - React framework for production
- [React](https://reactjs.org/) - A JavaScript library for building user interfaces
- [Docker](https://www.docker.com/) - Containerization platform
- [YARP](https://microsoft.github.io/reverse-proxy/) - Yet Another Reverse Proxy
- [Keycloak](https://www.keycloak.org/) - Open Source Identity and Access Management

## 📞 Support

- 📧 **Email**: [your-email@example.com]
- 🐛 **Issues**: [GitHub Issues](https://github.com/maaaruf/ecommerce-microservice/issues)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/maaaruf/ecommerce-microservice/discussions)

---

**Made with ❤️ by the E-commerce Microservices Team** 