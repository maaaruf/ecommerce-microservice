@echo off
REM Keycloak Setup Script for Windows
REM This batch file runs the PowerShell script to configure Keycloak

echo Setting up Keycloak for E-commerce Microservices...
echo.

REM Check if PowerShell is available
powershell -Command "Get-Host" >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: PowerShell is not available on this system.
    echo Please install PowerShell or use the bash script with Git Bash/WSL.
    pause
    exit /b 1
)

REM Check if the PowerShell script exists
if not exist "%~dp0setup-keycloak.ps1" (
    echo Error: setup-keycloak.ps1 not found in the scripts directory.
    echo Please ensure you're running this from the project root directory.
    pause
    exit /b 1
)

REM Run the PowerShell script
echo Running PowerShell setup script...
powershell -ExecutionPolicy Bypass -File "%~dp0setup-keycloak.ps1"

REM Check if the script ran successfully
if %errorlevel% neq 0 (
    echo.
    echo Error: Keycloak setup failed. Please check the error messages above.
    pause
    exit /b 1
)

echo.
echo Keycloak setup completed successfully!
echo.
echo Next steps:
echo 1. Update your configuration with the client secret shown above
echo 2. Restart the auth-service container
echo 3. Test the authentication flow
echo.
pause 