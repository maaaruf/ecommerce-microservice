FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Auth/Auth.API/Auth.API.csproj", "src/Services/Auth/Auth.API/"]
COPY ["src/Services/Auth/Auth.Application/Auth.Application.csproj", "src/Services/Auth/Auth.Application/"]
COPY ["src/Services/Auth/Auth.Domain/Auth.Domain.csproj", "src/Services/Auth/Auth.Domain/"]
COPY ["src/Services/Auth/Auth.Infrastructure/Auth.Infrastructure.csproj", "src/Services/Auth/Auth.Infrastructure/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "src/Shared/Shared.Contracts/"]
RUN dotnet restore "src/Services/Auth/Auth.API/Auth.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Auth/Auth.API"
RUN dotnet build "Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"] 