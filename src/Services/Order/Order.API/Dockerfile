FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Order/Order.API/Order.API.csproj", "src/Services/Order/Order.API/"]
COPY ["src/Services/Order/Order.Application/Order.Application.csproj", "src/Services/Order/Order.Application/"]
COPY ["src/Services/Order/Order.Domain/Order.Domain.csproj", "src/Services/Order/Order.Domain/"]
COPY ["src/Services/Order/Order.Infrastructure/Order.Infrastructure.csproj", "src/Services/Order/Order.Infrastructure/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "src/Shared/Shared.Contracts/"]
RUN dotnet restore "src/Services/Order/Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Order/Order.API"
RUN dotnet build "Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"] 