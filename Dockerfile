# Use the appropriate base images for your application
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image for building the application
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the project file and restore dependencies
COPY ["Temp/Temp.csproj", "Temp/"]
RUN dotnet restore "./Temp/Temp.csproj"

# Copy the remaining source code and build the application
COPY . .
WORKDIR "/src/Temp"
RUN dotnet build "./Temp.csproj" -c $BUILD_CONFIGURATION -o /app/build
# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Temp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the runtime image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Temp.dll"]