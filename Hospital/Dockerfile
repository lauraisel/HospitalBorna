# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Force IPv4 preference (if needed for DNS issues)
RUN echo "precedence ::ffff:0:0/96  100" >> /etc/gai.conf

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Hospital/Hospital.csproj", "Hospital/"]
RUN dotnet restore "Hospital/Hospital.csproj"

# Copy the rest of the source code and build
COPY . .
WORKDIR "/src/Hospital"
RUN dotnet build "Hospital.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
RUN dotnet publish "Hospital.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Force IPv4 preference
RUN echo "precedence ::ffff:0:0/96  100" >> /etc/gai.conf

# Set environment to Development so Swagger is enabled
ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app
COPY --from=build /app/publish .

# Listen on port 8080 on all interfaces
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

EXPOSE 8080

# Use non-root user if you have one set in your app (optional)
# USER $APP_UID

ENTRYPOINT ["dotnet", "Hospital.dll"]
