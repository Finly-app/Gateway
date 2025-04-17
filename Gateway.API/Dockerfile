# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]

# ✅ Correct way to copy everything from context (./Gateway)
COPY . .

# ✅ Use correct path to the csproj from context
RUN dotnet restore "Gateway.API/Gateway.API.csproj"
RUN dotnet build "Gateway.API/Gateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.API/Gateway.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.API.dll"]
