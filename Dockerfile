# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy the entire solution structure
COPY . .

# Restore dependencies for the API project (which will trigger restoration for all referenced projects)
RUN dotnet restore "Hoteling.API/Hoteling.API.csproj"

# Build and publish the API project
RUN dotnet publish "Hoteling.API/Hoteling.API.csproj" -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Create directory for SQLite if used
RUN mkdir -p /app/data

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Hoteling.API.dll"]
