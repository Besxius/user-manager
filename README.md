# UserManager API

## Overview
UserManager is an enterprise-grade RESTful web API engineered to provide secure user management, fine-grained role-based access control, and identity verification services. Developed on the .NET 8 framework, the platform strictly complies with Clean Architecture principles and incorporates Command Query Responsibility Segregation (CQRS) alongside advanced structural design patterns to ensure decoupled maintenance, high scalability, and seamless unit testing execution.

## System Architecture
The solution enforces a strict inner-directed dependency flow (The Dependency Rule), separating core business rules from external framework volatile infrastructures across four distinct layers:

### 1. UserManager.Domain
The structural core of the application, completely isolated from external frameworks, libraries, and runtime dependencies.
* **Entities:** Core domain schemas including User, Role, and UserProfile.
* **Constants:** Structural domain definitions such as UserRoles and UserStatuses.
* **Abstractions:** Pure contract specifications including IUserRepository, IRoleRepository, and IUserProfileRepository.

### 2. UserManager.Application
Encapsulates all orchestration and user-case automation behavior, serving as the mediator between presentation models and domain variants.
* **CQRS Implementations:** Business workflows are segregated cleanly into standalone Commands (write mutations) and Queries (idempotent read evaluations) using MediatR.
* **Cross-Cutting Pipelines:** Middleware behaviors such as ValidationBehavior and CommandLoggingBehavior capture requests to process pre-execution constraints seamlessly.
* **External Abstractions:** Interfaces defined for decoupled identity management and security layers (IJwtProvider, IPasswordHasher).

### 3. UserManager.Infrastructure
Implements technological adapters, handles physical database persistence, and integrates external utility frameworks.
* **MongoDB Persistence:** Configures low-level operational layers including MongoDbContext and MongoDbSettings, supported by an automated programmatic seeder (MongoDataSeeder).
* **Repository Implementation:** Concrete data mappings executing optimized aggregations against MongoDB database structures.
* **Security & Cryptography:** Direct implementation of JwtProvider for token issuance and PasswordHasher leveraging cryptographic standard hashing configurations.

### 4. UserManager.Api
The boundary presentation layer acting as the host entry point for managing routing architectures and client-side web requests.
* **Controllers:** High-performance RESTful structures categorized into AuthController, UsersController, and AdminController.
* **Global Error Middleware:** Intercepts out-of-band application exceptions via GlobalExceptionMiddleware to unify standard payload structures for client error states.
* **Configuration Setup:** Manages inversion-of-control container configurations, middleware orchestration, and authentication policy bindings.

## Functional Features

### Security and Identity Operations
* Structured user authentication containing secure registration and credentials validation pipelines.
* Protected communication contexts guarded by industry-standard JWT Bearer Token validation handlers.
* Decoupled access barriers enforcing role-based verification filters tailored for explicit User and Admin clearance levels.

### User Profile Management
* On-demand profile metadata extraction interfaces.
* Dynamic demographic information update workflows allowing customization of FullName, DateOfBirth, Gender, and Address.

### Administration Control Network
* Full visibility into application membership matrices and the global authorization roles network.
* Multicriteria filtering pipelines allowing data extractions by Role, Status, and Gender.
* Indexed identity query utilities targeting specific string matches for names or system emails.
* Account lifecycle management enabling immediate platform suspension or activation modifications.
* Administrative privilege assignment routines to update specific user identity tokens dynamically.

## Technology Stack and Core Libraries
* **Framework:** .NET Core 8.0 (ASP.NET Core Web API)
* **Database Target:** MongoDB Server Engine
* **CQRS Engine:** MediatR framework
* **Database Driver:** MongoDB.Driver official client library
* **Validation Middleware:** FluentValidation framework integration

---

## Installation and Execution Guide

### Prerequisites
* .NET 8.0 Software Development Kit (SDK) environment initialized local.
* Active MongoDB service instance running locally or hosted securely on MongoDB Atlas.
* Supported IDE workspace (Visual Studio 2022 or Visual Studio Code).

### 1. Database Connection Properties
Access the configuration mapping infrastructure within `UserManager.Api/appsettings.json` or `appsettings.Development.json` and adjust the MongoDB parameters to align with your setup:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "UserManagerDb"
  },
  "JwtSettings": {
    "Secret": "your_secure_cryptographic_jwt_key_configuration",
    "Issuer": "UserManagerApi",
    "Audience": "UserManagerClients"
  }
}
