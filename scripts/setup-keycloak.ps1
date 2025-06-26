# Keycloak Setup Script for E-commerce Microservices (Windows PowerShell)
# This script configures Keycloak with the required realm and client

Write-Host "Setting up Keycloak for E-commerce Microservices..." -ForegroundColor Green

# Wait for Keycloak to be ready
Write-Host "Waiting for Keycloak to be ready..." -ForegroundColor Yellow
do {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:8080/health/ready" -Method GET -TimeoutSec 5
        if ($response.StatusCode -eq 200) {
            break
        }
    }
    catch {
        Write-Host "Keycloak is not ready yet. Waiting..." -ForegroundColor Yellow
        Start-Sleep -Seconds 5
    }
} while ($true)

Write-Host "Keycloak is ready!" -ForegroundColor Green

# Get admin token
Write-Host "Getting admin token..." -ForegroundColor Yellow
$tokenBody = @{
    username = "admin"
    password = "admin"
    grant_type = "password"
    client_id = "admin-cli"
}

try {
    $tokenResponse = Invoke-RestMethod -Uri "http://localhost:8080/realms/master/protocol/openid-connect/token" `
        -Method POST `
        -Body $tokenBody `
        -ContentType "application/x-www-form-urlencoded"
    
    $ADMIN_TOKEN = $tokenResponse.access_token
    
    if (-not $ADMIN_TOKEN) {
        Write-Host "Failed to get admin token" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "Admin token obtained successfully" -ForegroundColor Green
}
catch {
    Write-Host "Failed to get admin token: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Create realm
Write-Host "Creating ecommerce realm..." -ForegroundColor Yellow
$realmBody = @{
    realm = "ecommerce"
    enabled = $true
    displayName = "E-commerce Microservices"
    displayNameHtml = "<div class=`"kc-logo-text`"><span>E-commerce</span></div>"
} | ConvertTo-Json

try {
    Invoke-RestMethod -Uri "http://localhost:8080/admin/realms" `
        -Method POST `
        -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN"; "Content-Type" = "application/json" } `
        -Body $realmBody
    Write-Host "Realm created successfully" -ForegroundColor Green
}
catch {
    Write-Host "Failed to create realm: $($_.Exception.Message)" -ForegroundColor Red
}

# Create client
Write-Host "Creating auth-service client..." -ForegroundColor Yellow
$clientBody = @{
    clientId = "auth-service"
    enabled = $true
    publicClient = $false
    standardFlowEnabled = $true
    directAccessGrantsEnabled = $true
    serviceAccountsEnabled = $true
    redirectUris = @("http://localhost:3000/callback", "http://localhost:3001/callback")
    webOrigins = @("http://localhost:3000", "http://localhost:3001")
    attributes = @{
        "saml.assertion.signature" = "false"
        "saml.force.post.binding" = "false"
        "saml.multivalued.roles" = "false"
        "saml.encrypt" = "false"
        "saml.server.signature" = "false"
        "saml.server.signature.keyinfo.ext" = "false"
        "exclude.session.state.from.auth.response" = "false"
        "saml_force_name_id_format" = "false"
        "saml.client.signature" = "false"
        "tls.client.certificate.bound.access.tokens" = "false"
        "saml.authnstatement" = "false"
        "display.on.consent.screen" = "false"
        "saml.onetimeuse.condition" = "false"
    }
} | ConvertTo-Json -Depth 10

try {
    Invoke-RestMethod -Uri "http://localhost:8080/admin/realms/ecommerce/clients" `
        -Method POST `
        -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN"; "Content-Type" = "application/json" } `
        -Body $clientBody
    Write-Host "Client created successfully" -ForegroundColor Green
}
catch {
    Write-Host "Failed to create client: $($_.Exception.Message)" -ForegroundColor Red
}

# Get client secret
Write-Host "Getting client secret..." -ForegroundColor Yellow
try {
    $clients = Invoke-RestMethod -Uri "http://localhost:8080/admin/realms/ecommerce/clients" `
        -Method GET `
        -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN" }
    
    $client = $clients | Where-Object { $_.clientId -eq "auth-service" }
    
    if ($client) {
        $clientSecretResponse = Invoke-RestMethod -Uri "http://localhost:8080/admin/realms/ecommerce/clients/$($client.id)/client-secret" `
            -Method GET `
            -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN" }
        
        $CLIENT_SECRET_VALUE = $clientSecretResponse.value
        Write-Host "Client Secret: $CLIENT_SECRET_VALUE" -ForegroundColor Green
        Write-Host "Please update your configuration with this client secret" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "Failed to get client secret: $($_.Exception.Message)" -ForegroundColor Red
}

# Create roles
Write-Host "Creating roles..." -ForegroundColor Yellow
$roles = @("User", "Admin", "Manager")

foreach ($role in $roles) {
    $roleBody = @{
        name = $role
        description = if ($role -eq "User") { "Regular user role" } elseif ($role -eq "Admin") { "Administrator role" } else { "Manager role" }
    } | ConvertTo-Json
    
    try {
        Invoke-RestMethod -Uri "http://localhost:8080/admin/realms/ecommerce/roles" `
            -Method POST `
            -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN"; "Content-Type" = "application/json" } `
            -Body $roleBody
        Write-Host "Role '$role' created successfully" -ForegroundColor Green
    }
    catch {
        Write-Host "Failed to create role '$role': $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Create a test user
Write-Host "Creating test user..." -ForegroundColor Yellow
$userBody = @{
    username = "testuser"
    email = "test@example.com"
    firstName = "Test"
    lastName = "User"
    enabled = $true
    emailVerified = $true
    credentials = @(
        @{
            type = "password"
            value = "Test123!"
            temporary = $false
        }
    )
} | ConvertTo-Json -Depth 10

try {
    Invoke-RestMethod -Uri "http://localhost:8080/admin/realms/ecommerce/users" `
        -Method POST `
        -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN"; "Content-Type" = "application/json" } `
        -Body $userBody
    Write-Host "Test user created successfully" -ForegroundColor Green
}
catch {
    Write-Host "Failed to create test user: $($_.Exception.Message)" -ForegroundColor Red
}

# Get user ID and assign role
try {
    $users = Invoke-RestMethod -Uri "http://localhost:8080/admin/realms/ecommerce/users" `
        -Method GET `
        -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN" }
    
    $user = $users | Where-Object { $_.username -eq "testuser" }
    
    if ($user) {
        $roles = Invoke-RestMethod -Uri "http://localhost:8080/admin/realms/ecommerce/roles" `
            -Method GET `
            -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN" }
        
        $userRole = $roles | Where-Object { $_.name -eq "User" }
        
        if ($userRole) {
            $roleMappingBody = @(
                @{
                    id = $userRole.id
                    name = $userRole.name
                }
            ) | ConvertTo-Json -Depth 10
            
            Invoke-RestMethod -Uri "http://localhost:8080/admin/realms/ecommerce/users/$($user.id)/role-mappings/realm" `
                -Method POST `
                -Headers @{ "Authorization" = "Bearer $ADMIN_TOKEN"; "Content-Type" = "application/json" } `
                -Body $roleMappingBody
            
            Write-Host "Role assigned to test user successfully" -ForegroundColor Green
        }
    }
}
catch {
    Write-Host "Failed to assign role to test user: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "Keycloak setup completed successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Keycloak Admin Console: http://localhost:8080" -ForegroundColor Cyan
Write-Host "Username: admin" -ForegroundColor White
Write-Host "Password: admin" -ForegroundColor White
Write-Host ""
Write-Host "Test User:" -ForegroundColor Cyan
Write-Host "Username: testuser" -ForegroundColor White
Write-Host "Password: Test123!" -ForegroundColor White
Write-Host "Email: test@example.com" -ForegroundColor White
Write-Host ""
Write-Host "Realm: ecommerce" -ForegroundColor Cyan
Write-Host "Client ID: auth-service" -ForegroundColor White
Write-Host "Client Secret: $CLIENT_SECRET_VALUE" -ForegroundColor White
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Update your configuration with the client secret above" -ForegroundColor White
Write-Host "2. Restart the auth-service container" -ForegroundColor White
Write-Host "3. Test the authentication flow" -ForegroundColor White 