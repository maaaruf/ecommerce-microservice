FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Notification/Notification.API/Notification.API.csproj", "src/Services/Notification/Notification.API/"]
COPY ["src/Services/Notification/Notification.Application/Notification.Application.csproj", "src/Services/Notification/Notification.Application/"]
COPY ["src/Services/Notification/Notification.Domain/Notification.Domain.csproj", "src/Services/Notification/Notification.Domain/"]
COPY ["src/Services/Notification/Notification.Infrastructure/Notification.Infrastructure.csproj", "src/Services/Notification/Notification.Infrastructure/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "src/Shared/Shared.Contracts/"]
RUN dotnet restore "src/Services/Notification/Notification.API/Notification.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Notification/Notification.API"
RUN dotnet build "Notification.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notification.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.API.dll"] 