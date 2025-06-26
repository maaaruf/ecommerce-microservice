#!/bin/bash

# Keycloak Setup Script for E-commerce Microservices
# This script configures Keycloak with the required realm and client

echo "Setting up Keycloak for E-commerce Microservices..."

# Wait for Keycloak to be ready
echo "Waiting for Keycloak to be ready..."
until curl -s http://localhost:8080/health/ready; do
    echo "Keycloak is not ready yet. Waiting..."
    sleep 5
done

echo "Keycloak is ready!"

# Get admin token
echo "Getting admin token..."
ADMIN_TOKEN=$(curl -s -X POST http://localhost:8080/realms/master/protocol/openid-connect/token \
    -H "Content-Type: application/x-www-form-urlencoded" \
    -d "username=admin" \
    -d "password=admin" \
    -d "grant_type=password" \
    -d "client_id=admin-cli" | jq -r '.access_token')

if [ "$ADMIN_TOKEN" = "null" ] || [ -z "$ADMIN_TOKEN" ]; then
    echo "Failed to get admin token"
    exit 1
fi

echo "Admin token obtained successfully"

# Create realm
echo "Creating ecommerce realm..."
curl -s -X POST http://localhost:8080/admin/realms \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{
        "realm": "ecommerce",
        "enabled": true,
        "displayName": "E-commerce Microservices",
        "displayNameHtml": "<div class=\"kc-logo-text\"><span>E-commerce</span></div>"
    }'

# Create client
echo "Creating auth-service client..."
curl -s -X POST http://localhost:8080/admin/realms/ecommerce/clients \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{
        "clientId": "auth-service",
        "enabled": true,
        "publicClient": false,
        "standardFlowEnabled": true,
        "directAccessGrantsEnabled": true,
        "serviceAccountsEnabled": true,
        "redirectUris": ["http://localhost:3000/callback", "http://localhost:3001/callback"],
        "webOrigins": ["http://localhost:3000", "http://localhost:3001"],
        "attributes": {
            "saml.assertion.signature": "false",
            "saml.force.post.binding": "false",
            "saml.multivalued.roles": "false",
            "saml.encrypt": "false",
            "saml.server.signature": "false",
            "saml.server.signature.keyinfo.ext": "false",
            "exclude.session.state.from.auth.response": "false",
            "saml_force_name_id_format": "false",
            "saml.client.signature": "false",
            "tls.client.certificate.bound.access.tokens": "false",
            "saml.authnstatement": "false",
            "display.on.consent.screen": "false",
            "saml.onetimeuse.condition": "false"
        }
    }'

# Get client secret
echo "Getting client secret..."
CLIENT_SECRET=$(curl -s -X GET http://localhost:8080/admin/realms/ecommerce/clients \
    -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.[] | select(.clientId == "auth-service") | .id')

if [ "$CLIENT_SECRET" != "null" ] && [ -n "$CLIENT_SECRET" ]; then
    CLIENT_SECRET_VALUE=$(curl -s -X GET http://localhost:8080/admin/realms/ecommerce/clients/$CLIENT_SECRET/client-secret \
        -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.value')
    
    echo "Client Secret: $CLIENT_SECRET_VALUE"
    echo "Please update your configuration with this client secret"
fi

# Create roles
echo "Creating roles..."
curl -s -X POST http://localhost:8080/admin/realms/ecommerce/roles \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{"name": "User", "description": "Regular user role"}'

curl -s -X POST http://localhost:8080/admin/realms/ecommerce/roles \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{"name": "Admin", "description": "Administrator role"}'

curl -s -X POST http://localhost:8080/admin/realms/ecommerce/roles \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{"name": "Manager", "description": "Manager role"}'

# Create a test user
echo "Creating test user..."
curl -s -X POST http://localhost:8080/admin/realms/ecommerce/users \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{
        "username": "testuser",
        "email": "test@example.com",
        "firstName": "Test",
        "lastName": "User",
        "enabled": true,
        "emailVerified": true,
        "credentials": [{
            "type": "password",
            "value": "Test123!",
            "temporary": false
        }]
    }'

# Get user ID and assign role
USER_ID=$(curl -s -X GET http://localhost:8080/admin/realms/ecommerce/users \
    -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.[] | select(.username == "testuser") | .id')

if [ "$USER_ID" != "null" ] && [ -n "$USER_ID" ]; then
    ROLE_ID=$(curl -s -X GET http://localhost:8080/admin/realms/ecommerce/roles \
        -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.[] | select(.name == "User") | .id')
    
    if [ "$ROLE_ID" != "null" ] && [ -n "$ROLE_ID" ]; then
        curl -s -X POST http://localhost:8080/admin/realms/ecommerce/users/$USER_ID/role-mappings/realm \
            -H "Authorization: Bearer $ADMIN_TOKEN" \
            -H "Content-Type: application/json" \
            -d "[{\"id\":\"$ROLE_ID\",\"name\":\"User\"}]"
    fi
fi

echo "Keycloak setup completed successfully!"
echo ""
echo "Keycloak Admin Console: http://localhost:8080"
echo "Username: admin"
echo "Password: admin"
echo ""
echo "Test User:"
echo "Username: testuser"
echo "Password: Test123!"
echo "Email: test@example.com"
echo ""
echo "Realm: ecommerce"
echo "Client ID: auth-service"
echo "Client Secret: $CLIENT_SECRET_VALUE" 