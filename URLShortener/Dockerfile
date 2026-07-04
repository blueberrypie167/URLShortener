# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["URLShortener/URLShortener.csproj", "URLShortener/"]
RUN dotnet restore "URLShortener/URLShortener.csproj"

# Copy remaining source code
COPY . .
WORKDIR "/src/URLShortener"
RUN dotnet publish "URLShortener.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Railway dynamically injects PORT)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "URLShortener.dll"]