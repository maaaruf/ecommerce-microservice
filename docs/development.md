# Development Guide

## Prerequisites

Before you begin development, ensure you have the following installed:

### Required Software
- **.NET 8 SDK**: [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Docker Desktop**: [Download here](https://www.docker.com/products/docker-desktop)
- **Node.js 18+**: [Download here](https://nodejs.org/)
- **Git**: [Download here](https://git-scm.com/)

### Optional Software
- **Visual Studio 2022** or **VS Code**: For .NET development
- **Postman** or **Insomnia**: For API testing
- **Redis Desktop Manager**: For Redis management
- **MongoDB Compass**: For MongoDB management

## Local Development Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd Ecommerce-microservice
```

### 2. Start Infrastructure Services
```bash
# Start all infrastructure services (PostgreSQL, Redis, MongoDB, RabbitMQ, Keycloak, ELK Stack)
docker-compose up -d
```

### 3. Configure Environment Variables
Create environment-specific configuration files:

**Auth Service** (`src/Services/Auth/Auth.API/appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=auth_db;Username=auth_user;Password=auth_password"
  },
  "Jwt": {
    "Key": "your-super-secret-key-with-at-least-32-characters",
    "Issuer": "https://localhost:5001",
    "Audience": "https://localhost:3000",
    "ExpiryInMinutes": 60
  }
}
```

### 4. Run Database Migrations
```bash
# Auth Service
cd src/Services/Auth/Auth.API
dotnet ef database update

# Product Service
cd src/Services/Product/Product.API
dotnet ef database update

# Order Service
cd src/Services/Order/Order.API
dotnet ef database update

# Payment Service
cd src/Services/Payment/Payment.API
dotnet ef database update

# Notification Service
cd src/Services/Notification/Notification.API
dotnet ef database update
```

### 5. Start Backend Services
```bash
# Start all services in separate terminals
dotnet run --project src/Services/Auth/Auth.API
dotnet run --project src/Services/Product/Product.API
dotnet run --project src/Services/Cart/Cart.API
dotnet run --project src/Services/Order/Order.API
dotnet run --project src/Services/Payment/Payment.API
dotnet run --project src/Services/Notification/Notification.API
dotnet run --project src/Gateway/API.Gateway
```

### 6. Start Frontend
```bash
cd src/Web/ClientApp
npm install
npm start
```

## Development Workflow

### 1. Code Organization
Follow the Clean Architecture pattern:

```
src/Services/{ServiceName}/
├── {ServiceName}.API/           # Controllers, Program.cs, appsettings.json
├── {ServiceName}.Application/   # Use cases, interfaces, services
├── {ServiceName}.Domain/        # Entities, value objects, domain services
└── {ServiceName}.Infrastructure/# Repositories, external services, database
```

### 2. Adding New Features
1. **Domain Layer**: Define entities, value objects, and domain services
2. **Application Layer**: Create use cases and application services
3. **Infrastructure Layer**: Implement repositories and external integrations
4. **API Layer**: Add controllers and endpoints
5. **Tests**: Write unit and integration tests

### 3. API Development
- Use **MediatR** for CQRS pattern
- Implement **FluentValidation** for request validation
- Use **AutoMapper** for object mapping
- Follow RESTful conventions

### 4. Database Development
- Use **Entity Framework Core** for data access
- Create migrations for schema changes
- Use **Fluent API** for complex configurations
- Implement repository pattern

### 5. Testing
```bash
# Run all tests
dotnet test

# Run specific service tests
dotnet test src/Services/Auth/Auth.Tests

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Best Practices

### 1. Code Quality
- Follow **SOLID principles**
- Use **Domain-Driven Design** patterns
- Implement **CQRS** where appropriate
- Write **clean, readable code**
- Use **meaningful names** for variables and methods

### 2. Error Handling
- Use **global exception handling**
- Return **consistent error responses**
- Log **structured error information**
- Implement **retry policies** for transient failures

### 3. Security
- Validate **all inputs**
- Use **HTTPS** in production
- Implement **proper authentication**
- Follow **OWASP guidelines**
- Use **secure configuration management**

### 4. Performance
- Use **async/await** for I/O operations
- Implement **caching** strategies
- Optimize **database queries**
- Use **connection pooling**
- Monitor **performance metrics**

### 5. Logging
- Use **structured logging** with Serilog
- Include **correlation IDs** for request tracing
- Log **business events** and **errors**
- Configure **appropriate log levels**

## Debugging

### 1. Backend Debugging
- Use **Visual Studio** or **VS Code** debugging
- Set breakpoints in controllers and services
- Use **logging** for troubleshooting
- Check **database connections** and queries

### 2. Frontend Debugging
- Use **React Developer Tools**
- Check **Redux DevTools** for state management
- Monitor **network requests** in browser dev tools
- Use **console logging** for debugging

### 3. Infrastructure Debugging
- Check **Docker container logs**
- Monitor **database connections**
- Verify **service discovery**
- Check **message queue** status

## Common Issues and Solutions

### 1. Database Connection Issues
```bash
# Check if PostgreSQL is running
docker ps | grep postgres

# Check connection string
# Ensure database exists and user has permissions
```

### 2. Port Conflicts
```bash
# Check which process is using a port
netstat -ano | findstr :5001

# Kill the process if needed
taskkill /PID <process_id> /F
```

### 3. Docker Issues
```bash
# Restart Docker Desktop
# Clear Docker cache
docker system prune -a

# Rebuild containers
docker-compose down
docker-compose up --build
```

### 4. Frontend Build Issues
```bash
# Clear npm cache
npm cache clean --force

# Delete node_modules and reinstall
rm -rf node_modules package-lock.json
npm install
```

## Development Tools

### 1. IDE Extensions (VS Code)
- **C#**: Microsoft C# extension
- **C# Dev Kit**: Enhanced C# development
- **Docker**: Docker extension
- **Thunder Client**: API testing
- **GitLens**: Enhanced Git integration

### 2. Useful Commands
```bash
# Build solution
dotnet build

# Run tests
dotnet test

# Add migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Generate API documentation
dotnet build
dotnet run --project src/Gateway/API.Gateway
```

### 3. Monitoring Tools
- **Kibana**: Log visualization (http://localhost:5601)
- **RabbitMQ Management**: Message queue management (http://localhost:15672)
- **Keycloak Admin**: Identity management (http://localhost:8080)

## Contributing

### 1. Git Workflow
- Create **feature branches** from main
- Use **conventional commits**
- Write **descriptive commit messages**
- Create **pull requests** for review

### 2. Code Review
- Review **code quality** and **architecture**
- Check **test coverage**
- Verify **security** considerations
- Ensure **documentation** is updated

### 3. Testing Requirements
- **Unit tests** for business logic
- **Integration tests** for APIs
- **End-to-end tests** for critical flows
- **Performance tests** for high-traffic scenarios 