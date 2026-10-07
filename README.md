# Enterprise B2B Stock and Order Management API

A RESTful Web API designed for B2B order processing, dynamic inventory control, and central error management. Built on ASP.NET Core with Entity Framework Core, JWT authorization, and FluentValidation.

## Core Features

- Inventory Management: Real-time stock validation prior to order creation and automatic stock restoration upon cancellation.
- Security: JWT-based authentication with role and claim handling via custom middleware guards.
- Input Validation: Decoupled request validation pipeline using FluentValidation to maintain domain integrity.
- Error Handling: Centralized global exception middleware delivering standardized HTTP response structures (Problem Details).
- Architecture: Clean DTO pattern implementation to isolate domain entities and prevent circular reference issues.

## Tech Stack

- Framework: .NET 8 / ASP.NET Core Web API
- Database & ORM: SQL Server, Entity Framework Core (Code-First)
- Security: System.IdentityModel.Tokens.Jwt
- Validation: FluentValidation.DependencyInjectionExtensions
- API Documentation: Swagger / OpenAPI

## Setup and Execution

### Prerequisites

- .NET 8 SDK
- SQL Server or LocalDB

### Installation

1. Clone the repository:
   git clone https://github.com/KuzeyKaraseyfioglu/StockOrderApi.git
   cd StockOrderApi

2. Configure the database connection string in appsettings.json.

3. Run EF Core database migrations:
   dotnet ef database update

4. Start the application:
   dotnet run

5. Open the interactive API documentation at:
   https://localhost:7123/swagger

## Authentication Workflow

1. Send a POST request to /api/Auth/login with valid user credentials.
2. Extract the JWT token from the JSON response body.
3. Attach the token to subsequent request headers:
   Authorization: Bearer <your_jwt_token>
