# E-commerce Microservices Platform

A modern, scalable e-commerce platform built with .NET 8 microservices, React frontend, and cloud-native technologies.

## 🏗️ Architecture Overview

### Backend Services
- **Auth Service**: User authentication and authorization via Keycloak
- **Product Service**: Product catalog, inventory management, and search
- **Cart Service**: Shopping cart management with Redis-backed sessions
- **Order Service**: Order lifecycle management
- **Payment Service**: Payment processing with multiple gateway support
- **Notification Service**: Email, SMS, and webhook notifications
- **API Gateway**: YARP-based gateway for service aggregation

### Frontend
- **React SPA**: Modern UI with Redux Toolkit and TailwindCSS
- **Role-based access**: JWT-based authorization
- **Real-time updates**: WebSocket integration

### Infrastructure
- **Database**: PostgreSQL (per service), Redis (caching/pub-sub), MongoDB (catalog)
- **Message Broker**: RabbitMQ for async communication
- **Identity**: Keycloak OAuth2/OIDC
- **Containerization**: Docker + Docker Compose
- **Orchestration**: Kubernetes (optional for dev, recommended for prod)
- **CI/CD**: GitHub Actions

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- Docker Desktop
- Node.js 18+
- PostgreSQL
- Redis
- MongoDB

### Local Development
```bash
# Clone the repository
git clone <repository-url>
cd Ecommerce-microservice

# Start infrastructure services
docker-compose up -d

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
│   ├── Services/           # Microservices
│   │   ├── Auth/          # Authentication service
│   │   ├── Product/       # Product catalog service
│   │   ├── Cart/          # Shopping cart service
│   │   ├── Order/         # Order management service
│   │   ├── Payment/       # Payment processing service
│   │   └── Notification/  # Notification service
│   ├── Gateway/           # API Gateway (YARP)
│   ├── Shared/            # Shared libraries and contracts
│   └── Web/               # React frontend
├── infrastructure/        # Infrastructure configuration
├── docs/                  # Documentation
└── scripts/              # Build and deployment scripts
```

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run specific service tests
dotnet test src/Services/Auth/Auth.Tests
```

## 📚 Documentation

- [Architecture Guide](docs/architecture.md)
- [Development Guide](docs/development.md)
- [Deployment Guide](docs/deployment.md)
- [API Documentation](docs/api.md)

## 🤝 Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 