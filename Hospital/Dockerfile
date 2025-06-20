# Base runtime image (used for running the app)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Force IPv4 preference to avoid IPv6 DNS issues with Supabase Shared Pooler
RUN echo "precedence ::ffff:0:0/96  100" >> /etc/gai.conf
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Build image (used for building/publishing the app)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Force IPv4 in the build stage too, if needed for restore steps
RUN echo "precedence ::ffff:0:0/96  100" >> /etc/gai.conf
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hospital/Hospital.csproj", "Hospital/"]
RUN dotnet restore "./Hospital/Hospital.csproj"
COPY . .
WORKDIR "/src/Hospital"
RUN dotnet build "./Hospital.csproj" -c $BUILD_CONFIGURATION -o /app/build
