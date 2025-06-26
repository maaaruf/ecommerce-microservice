# Keycloak Integration Guide

This document explains how to set up and use Keycloak for authentication in the E-commerce Microservices platform.

## Overview

The Auth service has been refactored to use Keycloak as the OAuth2/OIDC provider instead of custom JWT authentication. This provides:

- **Enterprise-grade identity management**
- **OAuth2/OIDC compliance**
- **Built-in user management**
- **Advanced security features**
- **Centralized authentication**

## Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Auth Service  │    │    Keycloak     │
│   (Next.js/     │◄──►│   (.NET 8)      │◄──►│   (OAuth2/OIDC) │
│   React)        │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## Setup Instructions

### 1. Start Keycloak

Keycloak is already configured in `docker-compose.yml` and will start automatically:

```bash
docker-compose up -d keycloak
```

### 2. Configure Keycloak

Choose the appropriate setup script for your operating system:

#### **Windows Users**

**Option A: PowerShell Script (Recommended)**
```powershell
# Run from project root directory
.\scripts\setup-keycloak.ps1
```

**Option B: Batch File (Easiest)**
```cmd
# Run from project root directory
scripts\setup-keycloak.bat
```

**Option C: Git Bash/WSL**
```bash
# If you have Git Bash or WSL installed
bash scripts/setup-keycloak.sh
```

#### **Linux/Mac Users**
```bash
# Make script executable (first time only)
chmod +x scripts/setup-keycloak.sh

# Run the script
./scripts/setup-keycloak.sh
```

### 3. Update Configuration

After running the setup script, update the client secret in your configuration:

```json
{
  "Keycloak": {
    "ClientSecret": "your-actual-client-secret-from-script"
  }
}
```

## Keycloak Configuration

### Realm: `ecommerce`

The setup script creates a realm called `ecommerce` with the following configuration:

- **Display Name**: E-commerce Microservices
- **Enabled**: Yes
- **Login Theme**: Default

### Client: `auth-service`

The client is configured with:

- **Client ID**: `auth-service`
- **Client Type**: Confidential
- **Standard Flow**: Enabled
- **Direct Access Grants**: Enabled
- **Service Accounts**: Enabled
- **Redirect URIs**: 
  - `http://localhost:3000/callback`
  - `http://localhost:3001/callback`
- **Web Origins**:
  - `http://localhost:3000`
  - `http://localhost:3001`

### Roles

The following roles are created:

- **User**: Regular user role
- **Admin**: Administrator role  
- **Manager**: Manager role

### Test User

A test user is created with:

- **Username**: `testuser`
- **Password**: `Test123!`
- **Email**: `test@example.com`
- **Role**: User

## API Endpoints

### OAuth2/OIDC Flow

#### 1. Get Authorization URL
```http
GET /api/auth/login?redirectUri={redirectUri}&state={state}
```

Returns the Keycloak authorization URL for login.

#### 2. Handle Callback
```http
GET /api/auth/callback?code={code}&state={state}&redirectUri={redirectUri}
```

Exchanges the authorization code for tokens and creates/updates the user.

#### 3. Refresh Token
```http
POST /api/auth/refresh
Content-Type: application/json

"refresh_token_here"
```

Refreshes the access token using the refresh token.

#### 4. Validate Token
```http
POST /api/auth/validate
Content-Type: application/json

"access_token_here"
```

Validates an access token with Keycloak.

#### 5. Get User Info
```http
POST /api/auth/userinfo
Content-Type: application/json

"access_token_here"
```

Gets user information from Keycloak.

#### 6. Logout
```http
POST /api/auth/logout
Content-Type: application/json

"refresh_token_here"
```

Revokes the refresh token.

#### 7. Get Logout URL
```http
GET /api/auth/logout-url?redirectUri={redirectUri}&idToken={idToken}
```

Returns the Keycloak logout URL.

### User Management

#### Get Profile
```http
GET /api/auth/profile
Authorization: Bearer {access_token}
```

Gets the current user's profile.

#### Update Profile
```http
PUT /api/auth/profile
Authorization: Bearer {access_token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "profilePictureUrl": "https://example.com/avatar.jpg",
  "timeZone": "UTC",
  "language": "en"
}
```

Updates the user's profile.

## Frontend Integration

### Next.js Storefront

```typescript
// Get login URL
const loginUrl = await fetch('/api/auth/login?redirectUri=http://localhost:3000/callback&state=random-state');

// Handle callback
const response = await fetch(`/api/auth/callback?code=${code}&state=${state}&redirectUri=${redirectUri}`);
const { accessToken, refreshToken, user } = await response.json();

// Use tokens for authenticated requests
const profile = await fetch('/api/auth/profile', {
  headers: { Authorization: `Bearer ${accessToken}` }
});
```

### React Admin Panel

```typescript
// Similar integration as Next.js
// Use the same endpoints for authentication
```

## Migration from Custom JWT

### What Changed

1. **Authentication Flow**: Now uses OAuth2/OIDC instead of custom JWT
2. **User Management**: Handled by Keycloak instead of custom implementation
3. **Token Validation**: Validated by Keycloak instead of custom logic
4. **Password Management**: Handled by Keycloak admin interface

### Legacy Endpoints

The following legacy endpoints are still available but return error messages directing users to use Keycloak:

- `POST /api/auth/register`
- `POST /api/auth/login-legacy`
- `POST /api/auth/forgot-password`
- `POST /api/auth/reset-password`
- `POST /api/auth/change-password`
- `POST /api/auth/verify-email`
- `POST /api/auth/resend-email-verification`
- `GET /api/auth/two-factor/setup`
- `POST /api/auth/two-factor/enable`
- `POST /api/auth/two-factor/disable`

## Security Considerations

1. **HTTPS**: Use HTTPS in production
2. **Client Secret**: Keep the client secret secure
3. **Redirect URIs**: Only allow trusted redirect URIs
4. **Token Storage**: Store tokens securely (httpOnly cookies recommended)
5. **Token Validation**: Always validate tokens on the server side

## Troubleshooting

### Common Issues

1. **Keycloak not accessible**: Check if Keycloak container is running
2. **Invalid client secret**: Update the configuration with the correct secret
3. **Redirect URI mismatch**: Ensure redirect URIs match exactly
4. **CORS issues**: Check web origins configuration

### Windows-Specific Issues

1. **PowerShell Execution Policy**: If you get execution policy errors, run:
   ```powershell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```

2. **PowerShell Not Found**: Ensure PowerShell is installed (Windows 10+ includes it by default)

3. **Script Location**: Make sure you're running the script from the project root directory

### Debug Commands

```bash
# Check Keycloak health
curl http://localhost:8080/health/ready

# Check Keycloak logs
docker logs keycloak

# Check Auth service logs
docker logs auth-service
```

**Windows PowerShell:**
```powershell
# Check Keycloak health
Invoke-WebRequest -Uri "http://localhost:8080/health/ready" -Method GET

# Check Keycloak logs
docker logs keycloak

# Check Auth service logs
docker logs auth-service
```

## Next Steps

1. **Frontend Integration**: Update frontend applications to use OAuth2 flow
2. **User Management**: Use Keycloak admin interface for user management
3. **Role Management**: Configure roles and permissions in Keycloak
4. **Production Setup**: Configure production Keycloak instance
5. **Monitoring**: Set up monitoring for authentication flows 