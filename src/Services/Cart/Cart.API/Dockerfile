FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Cart/Cart.API/Cart.API.csproj", "src/Services/Cart/Cart.API/"]
COPY ["src/Services/Cart/Cart.Application/Cart.Application.csproj", "src/Services/Cart/Cart.Application/"]
COPY ["src/Services/Cart/Cart.Domain/Cart.Domain.csproj", "src/Services/Cart/Cart.Domain/"]
COPY ["src/Services/Cart/Cart.Infrastructure/Cart.Infrastructure.csproj", "src/Services/Cart/Cart.Infrastructure/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "src/Shared/Shared.Contracts/"]
RUN dotnet restore "src/Services/Cart/Cart.API/Cart.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Cart/Cart.API"
RUN dotnet build "Cart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cart.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cart.API.dll"] 