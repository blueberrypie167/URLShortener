# URL Shortener API

## Overview
A lightweight REST API that creates short URLs which redirect to the original address, following a BRD → PRD → backlog → implementation workflow.

## Features
- Shorten long URLs
- Redirect using short codes
- Optional expiry duration
- Optional custom aliases
- URL validation

## Tech Stack
- ASP.NET Core
- C#
- Entity Framework Core
- PostgreSQL
- Swagger/OpenAPI

## API Endpoints

POST /shorten
Creates a shortened URL.

GET /{code}
Redirects to the original URL.

## Running the Project

1. Clone the repository
2. Update the connection string
3. Run database migrations
4. Start the API
5. Open Swagger (or frontend at http:localhost:5227/)

## Future Improvements
- Authentication
- Analytics
- Rate limiting
- QR code generation
- Link editing
