# Contributing to E-commerce Microservices Platform

Thank you for your interest in contributing to our e-commerce microservices platform! This document provides guidelines and information for contributors.

## Code of Conduct

By participating in this project, you agree to abide by our Code of Conduct. Please read it before contributing.

## How to Contribute

### 1. Reporting Issues

Before creating an issue, please:

- Check if the issue has already been reported
- Use the appropriate issue template
- Provide detailed information including:
  - Steps to reproduce
  - Expected vs actual behavior
  - Environment details
  - Screenshots (if applicable)

### 2. Feature Requests

When requesting features:

- Describe the feature clearly
- Explain the use case and benefits
- Consider implementation complexity
- Check if similar features exist

### 3. Pull Requests

#### Before Submitting a PR

1. **Fork the repository**
2. **Create a feature branch** from `main`
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Follow coding standards** (see below)
4. **Write tests** for new functionality
5. **Update documentation** as needed
6. **Ensure all tests pass**
7. **Update the changelog** if applicable

#### PR Guidelines

- Use **conventional commits** for commit messages
- Write **descriptive PR titles** and descriptions
- Include **screenshots** for UI changes
- Reference **related issues**
- Request **appropriate reviewers**

#### Conventional Commits

Use the following format for commit messages:

```
type(scope): description

[optional body]

[optional footer]
```

Types:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes
- `refactor`: Code refactoring
- `test`: Test changes
- `chore`: Build/tooling changes

Examples:
```
feat(auth): add JWT token refresh functionality
fix(product): resolve inventory update race condition
docs(api): update authentication endpoint documentation
```

## Development Setup

### Prerequisites

- .NET 8 SDK
- Docker Desktop
- Node.js 18+
- Git

### Local Development

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Ecommerce-microservice
   ```

2. **Start infrastructure services**
   ```bash
   docker-compose up -d
   ```

3. **Run database migrations**
   ```bash
   dotnet ef database update --project src/Services/Auth/Auth.API
   # Repeat for other services
   ```

4. **Start services**
   ```bash
   dotnet run --project src/Services/Auth/Auth.API
   # Start other services in separate terminals
   ```

5. **Start frontend**
   ```bash
   cd src/Web/ClientApp
   npm install
   npm start
   ```

## Coding Standards

### .NET Backend

#### Code Style
- Follow **Microsoft C# coding conventions**
- Use **PascalCase** for public members
- Use **camelCase** for private fields
- Use **meaningful names** for variables and methods
- Keep methods **small and focused** (max 20 lines)
- Use **async/await** for I/O operations

#### Architecture
- Follow **Clean Architecture** principles
- Use **Domain-Driven Design** patterns
- Implement **SOLID principles**
- Use **dependency injection**
- Follow **CQRS** pattern with MediatR

#### Testing
- Write **unit tests** for business logic
- Write **integration tests** for APIs
- Aim for **80%+ code coverage**
- Use **meaningful test names**
- Follow **AAA pattern** (Arrange, Act, Assert)

Example test:
```csharp
[Fact]
public async Task RegisterUser_WithValidData_ShouldCreateUser()
{
    // Arrange
    var request = new CreateUserRequest
    {
        Email = "test@example.com",
        Username = "testuser",
        Password = "password123"
    };

    // Act
    var result = await _authService.RegisterUserAsync(request);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(request.Email, result.Email);
}
```

### React Frontend

#### Code Style
- Use **TypeScript** for type safety
- Follow **ESLint** and **Prettier** configurations
- Use **functional components** with hooks
- Use **PascalCase** for components
- Use **camelCase** for variables and functions

#### State Management
- Use **Redux Toolkit** for global state
- Use **local state** for component-specific data
- Follow **immutable update patterns**
- Use **selectors** for derived state

#### Component Structure
```typescript
// Component structure
interface ComponentProps {
  // Props interface
}

export const Component: React.FC<ComponentProps> = ({ prop1, prop2 }) => {
  // Hooks
  const [state, setState] = useState();
  const dispatch = useAppDispatch();
  const data = useAppSelector(selectData);

  // Event handlers
  const handleClick = () => {
    // Handler logic
  };

  // Render
  return (
    <div>
      {/* JSX */}
    </div>
  );
};
```

## Testing Guidelines

### Backend Testing

#### Unit Tests
- Test **business logic** in isolation
- Mock **external dependencies**
- Test **edge cases** and error scenarios
- Use **test data builders** for complex objects

#### Integration Tests
- Test **API endpoints**
- Use **test databases**
- Test **authentication and authorization**
- Verify **response formats**

#### Test Organization
```
Tests/
├── Unit/
│   ├── Domain/
│   ├── Application/
│   └── Infrastructure/
└── Integration/
    ├── API/
    └── Database/
```

### Frontend Testing

#### Unit Tests
- Test **component rendering**
- Test **user interactions**
- Test **Redux actions and reducers**
- Mock **API calls**

#### Integration Tests
- Test **user workflows**
- Test **API integration**
- Test **routing**

## Documentation

### Code Documentation
- Write **XML documentation** for public APIs
- Use **meaningful comments** for complex logic
- Document **business rules** and assumptions
- Keep documentation **up to date**

### API Documentation
- Use **Swagger/OpenAPI** for API documentation
- Include **request/response examples**
- Document **error codes** and messages
- Keep **endpoint documentation** current

### Architecture Documentation
- Update **architecture diagrams**
- Document **design decisions**
- Maintain **deployment guides**
- Keep **README files** current

## Review Process

### Code Review Checklist

#### Functionality
- [ ] Code works as intended
- [ ] All tests pass
- [ ] No breaking changes
- [ ] Performance considerations

#### Code Quality
- [ ] Follows coding standards
- [ ] Proper error handling
- [ ] Security considerations
- [ ] No code duplication

#### Documentation
- [ ] Code is self-documenting
- [ ] API documentation updated
- [ ] README updated if needed
- [ ] Comments for complex logic

#### Testing
- [ ] Adequate test coverage
- [ ] Tests are meaningful
- [ ] Integration tests included
- [ ] Edge cases covered

### Review Guidelines

#### For Reviewers
- Be **constructive** and **respectful**
- Focus on **code quality** and **functionality**
- Ask **clarifying questions**
- Suggest **improvements**
- Approve only when **satisfied**

#### For Authors
- Respond to **review comments**
- Make **requested changes**
- Explain **design decisions**
- Be **open to feedback**
- Keep **PRs focused** and **small**

## Release Process

### Versioning
We follow **Semantic Versioning** (SemVer):
- **MAJOR**: Breaking changes
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes (backward compatible)

### Release Checklist
- [ ] All tests passing
- [ ] Documentation updated
- [ ] Changelog updated
- [ ] Version bumped
- [ ] Release notes prepared
- [ ] Deployment tested

## Getting Help

### Communication Channels
- **GitHub Issues**: For bugs and feature requests
- **GitHub Discussions**: For questions and ideas
- **Pull Requests**: For code reviews and discussions

### Resources
- [Architecture Guide](docs/architecture.md)
- [Development Guide](docs/development.md)
- [Deployment Guide](docs/deployment.md)
- [API Documentation](docs/api.md)

## Recognition

Contributors will be recognized in:
- **README.md** contributors section
- **Release notes**
- **Project documentation**

Thank you for contributing to our e-commerce microservices platform! 