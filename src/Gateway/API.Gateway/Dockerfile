FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Gateway/API.Gateway/API.Gateway.csproj", "src/Gateway/API.Gateway/"]
RUN dotnet restore "src/Gateway/API.Gateway/API.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Gateway/API.Gateway"
RUN dotnet build "API.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Gateway.dll"] 