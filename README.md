# 🛒 E-commerce Microservices Platform

A modern, scalable e-commerce platform built with **.NET 8 microservices**, **React frontend**, and **cloud-native technologies**. This platform demonstrates best practices in microservices architecture, event-driven design, and modern web development.

## 🏗️ Architecture Overview

### Backend Services
| Service | Purpose | Technology Stack |
|---------|---------|------------------|
| **Auth Service** | User authentication and authorization | .NET 8, PostgreSQL, Keycloak, JWT |
| **Product Service** | Product catalog and inventory management | .NET 8, PostgreSQL, MongoDB, Elasticsearch |
| **Cart Service** | Shopping cart management | .NET 8, Redis, PostgreSQL |
| **Order Service** | Order lifecycle management | .NET 8, PostgreSQL, RabbitMQ |
| **Payment Service** | Payment processing | .NET 8, PostgreSQL, Stripe, RabbitMQ |
| **Notification Service** | Email, SMS, and webhook notifications | .NET 8, PostgreSQL, SendGrid, Twilio |
| **API Gateway** | Service aggregation and routing | .NET 8, YARP |

### Frontend
- **React SPA** with TypeScript
- **Redux Toolkit** for state management
- **TailwindCSS** for styling
- **Axios** for API communication
- **Role-based access** with JWT

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

# Check service status
docker-compose ps
```

**Access Points:**
- 🌐 **Frontend**: http://localhost:3000
- 🔌 **API Gateway**: http://localhost:5000
- 📊 **Kibana**: http://localhost:5601
- 🐰 **RabbitMQ Management**: http://localhost:15672
- 🔐 **Keycloak Admin**: http://localhost:8080

### Option 2: Local Development

```bash
# Start infrastructure services only
docker-compose up -d postgres-auth postgres-product postgres-cart postgres-order postgres-payment postgres-notification mongodb redis rabbitmq keycloak elasticsearch kibana

# Start backend services
dotnet run --project src/Services/Auth/Auth.API
dotnet run --project src/Services/Product/Product.API
dotnet run --project src/Services/Cart/Cart.API
dotnet run --project src/Services/Order/Order.API
dotnet run --project src/Services/Payment/Payment.API
dotnet run --project src/Services/Notification/Notification.API
dotnet run --project src/Gateway/API.Gateway

# Start frontend
cd src/Web/ClientApp
npm install
npm start
```

## 📁 Project Structure

```
├── src/
│   ├── Services/                    # Microservices
│   │   ├── Auth/                   # Authentication service
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
│   └── Web/                       # React frontend
│       └── ClientApp/             # React application
├── docs/                          # Documentation
├── infrastructure/                # Infrastructure configs
└── scripts/                       # Build and deployment scripts
```

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
- JWT-based authentication
- Role-based access control (RBAC)
- Integration with Keycloak
- Token refresh mechanism

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

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run specific service tests
dotnet test src/Services/Auth/Auth.Tests
dotnet test src/Services/Product/Product.Tests

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📚 Documentation

- [🏗️ Architecture Guide](docs/architecture.md) - Detailed architecture and design decisions
- [🛠️ Development Guide](docs/development.md) - Setup, workflow, and best practices
- [🚀 Deployment Guide](docs/deployment.md) - Production deployment with Docker and Kubernetes
- [🤝 Contributing Guide](CONTRIBUTING.md) - Guidelines for contributors

## 🔄 Development Workflow

### Adding New Features
1. **Domain Layer**: Define entities, value objects, and domain services
2. **Application Layer**: Create use cases and application services
3. **Infrastructure Layer**: Implement repositories and external integrations
4. **API Layer**: Add controllers and endpoints
5. **Tests**: Write unit and integration tests

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
kubectl apply -f k8s/frontend/
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