# Base runtime image (used for running the app)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN echo "precedence ::ffff:0:0/96  100" >> /etc/gai.conf
WORKDIR /app
EXPOSE 8080

# Build stage (used for building/publishing the app)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN echo "precedence ::ffff:0:0/96  100" >> /etc/gai.conf
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hospital/Hospital.csproj", "Hospital/"]
RUN dotnet restore "./Hospital/Hospital.csproj"
COPY . .
WORKDIR "/src/Hospital"
RUN dotnet build "./Hospital.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./Hospital.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage (runtime)
FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Development
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Hospital.dll"]