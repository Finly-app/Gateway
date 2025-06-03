# ---------- Base Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Create folder for HTTPS cert
RUN mkdir -p /https

# Install openssl for cert generation
RUN apt-get update && apt-get install -y openssl

# Generate a self-signed cert (valid for 10 years)
RUN openssl req -x509 -newkey rsa:2048 -nodes \
  -keyout /https/key.pem -out /https/cert.pem \
  -days 3650 -subj "/CN=localhost"

# Convert to PFX (needed by ASP.NET Core)
RUN openssl pkcs12 -export \
  -out /https/aspnetapp.pfx \
  -inkey /https/key.pem \
  -in /https/cert.pem \
  -passout pass:H1707DireWolf!?

EXPOSE 8080
EXPOSE 443

# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore
COPY . .
RUN dotnet restore "Gateway.API/Gateway.API.csproj"
RUN dotnet build "Gateway.API/Gateway.API.csproj" -c Release -o /app/build

# ---------- Publish Stage ----------
FROM build AS publish
RUN dotnet publish "Gateway.API/Gateway.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------- Final Stage ----------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=https://+:443;http://+:8080
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=H1707DireWolf!?
ENTRYPOINT ["dotnet", "Gateway.API.dll"]
