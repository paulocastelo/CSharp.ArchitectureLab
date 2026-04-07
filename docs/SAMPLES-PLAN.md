# Samples Plan — CSharp.ArchitectureLab

This document lists all samples to be implemented in this repository, their purpose, and the concepts each one demonstrates.

Each sample is a self-contained ASP.NET Core Web API project. They share the same inventory domain (products, categories) to keep the focus on the architectural pattern, not on learning a new domain.

---

## Sample List

### 01 — Dependency Injection

**Folder:** `samples/01-dependency-injection`

**Concepts demonstrated:**
- registering services with different lifetimes (Transient, Scoped, Singleton)
- constructor injection
- interface-based design
- why DI improves testability and decoupling

**What the sample contains:**
- a minimal API with a `ProductService` and `ICategoryRepository`
- three services registered with different lifetimes, each logging their instance ID to show when they are created and reused
- a README explaining the difference between the three lifetimes with practical consequences

---

### 02 — Repository Pattern

**Folder:** `samples/02-repository-pattern`

**Concepts demonstrated:**
- abstracting persistence behind an interface
- separating domain logic from data access
- in-memory repository for testing vs. EF Core repository for production
- dependency inversion principle in practice

**What the sample contains:**
- `IProductRepository` interface
- `InMemoryProductRepository` implementation (no database needed)
- `EfCoreProductRepository` implementation (with EF Core + SQLite)
- `ProductService` depending only on the interface
- switching implementations via DI configuration

---

### 03 — DTOs and Mapping

**Folder:** `samples/03-dto-mapping`

**Concepts demonstrated:**
- why DTOs are needed (separation of API contract from domain model)
- manual mapping vs. AutoMapper
- request DTOs for input validation
- response DTOs to control what is exposed

**What the sample contains:**
- `Product` domain entity with internal fields (e.g., `PasswordHash`, `InternalCost`)
- `CreateProductRequest` with validation annotations
- `ProductResponse` exposing only safe fields
- manual mapping extension method
- AutoMapper profile as an alternative

---

### 04 — Validation

**Folder:** `samples/04-validation`

**Concepts demonstrated:**
- Data Annotations for simple cases
- FluentValidation for complex rules
- centralized validation error response format
- why validation belongs at the API boundary

**What the sample contains:**
- `CreateProductRequest` validated with Data Annotations
- `RegisterUserRequest` validated with FluentValidation (custom rules: password strength, email uniqueness)
- global filter returning `400 Bad Request` with a consistent error body:
  ```json
  {
    "type": "validation_error",
    "errors": {
      "email": ["Email is required."],
      "password": ["Password must be at least 8 characters."]
    }
  }
  ```

---

### 05 — Error Handling

**Folder:** `samples/05-error-handling`

**Concepts demonstrated:**
- global exception handling middleware
- mapping exceptions to HTTP status codes
- consistent error response envelope
- distinguishing domain errors from infrastructure errors

**What the sample contains:**
- `ExceptionHandlingMiddleware` catching all unhandled exceptions
- domain exception types: `NotFoundException`, `ConflictException`, `BusinessRuleException`
- each exception mapped to the appropriate HTTP status (404, 409, 422)
- error response format:
  ```json
  {
    "type": "not_found",
    "detail": "Product with id 'abc' was not found."
  }
  ```
- an endpoint that intentionally throws each type to demonstrate the behavior

---

### 06 — Pagination

**Folder:** `samples/06-pagination`

**Concepts demonstrated:**
- offset-based pagination (page + pageSize)
- cursor-based pagination (for append-only datasets)
- pagination metadata in the response
- performance implications of each approach

**What the sample contains:**
- `GET /api/products?page=1&pageSize=10` returning:
  ```json
  {
    "items": [...],
    "page": 1,
    "pageSize": 10,
    "totalCount": 150,
    "totalPages": 15,
    "hasNextPage": true,
    "hasPreviousPage": false
  }
  ```
- `PagedResult<T>` generic wrapper
- EF Core implementation with `Skip` / `Take`
- a README note explaining when cursor pagination is preferable

---

### 07 — Unit Testing

**Folder:** `samples/07-unit-testing`

**Concepts demonstrated:**
- testing business rules in isolation
- mocking dependencies with Moq
- test naming convention: `Method_Scenario_ExpectedResult`
- Arrange / Act / Assert structure
- testing edge cases and error paths

**What the sample contains:**
- `StockMovementService` with real business rules (balance validation)
- tests with xUnit + Moq covering:
  - happy path (entry increases balance)
  - exit decreases balance
  - exit with insufficient balance throws
  - exit with exact balance succeeds
  - invalid quantity throws
- a README explaining why these tests do not hit a database

---

### 08 — Auth Basics (JWT)

**Folder:** `samples/08-auth-basics`

**Concepts demonstrated:**
- JWT generation and validation in ASP.NET Core
- `[Authorize]` attribute protecting endpoints
- claims-based identity (userId, email in the token)
- password hashing with BCrypt
- token expiration and refresh concept (without full refresh token flow)

**What the sample contains:**
- `POST /api/auth/register` — creates user with hashed password
- `POST /api/auth/login` — validates credentials, returns JWT
- `GET /api/profile` — protected endpoint returning claims from token
- `JwtService` encapsulating generation and validation
- Swagger configured with Bearer token support

---

### 09 — Logging

**Folder:** `samples/09-logging`

**Concepts demonstrated:**
- structured logging with Serilog
- log levels and when to use each (Debug, Information, Warning, Error)
- request/response logging middleware
- correlation ID for tracing requests across logs
- why `Console.WriteLine` is not acceptable in production

**What the sample contains:**
- Serilog configured to write to console (structured JSON) and a rolling file
- `RequestLoggingMiddleware` logging method, path, status code, and duration
- `CorrelationIdMiddleware` injecting a unique ID per request
- examples of `_logger.LogInformation`, `LogWarning`, `LogError` in a service
- a README showing what the output looks like and how to search by correlation ID

---

### 10 — CQRS Basics

**Folder:** `samples/10-cqrs`

**Concepts demonstrated:**
- separating read and write operations (Commands vs. Queries)
- MediatR as the mediator/dispatcher
- why CQRS improves maintainability in complex systems
- the difference between CQRS and full event sourcing

**What the sample contains:**
- `CreateProductCommand` + `CreateProductCommandHandler`
- `GetProductByIdQuery` + `GetProductByIdQueryHandler`
- `GetProductsQuery` (paginated) + `GetProductsQueryHandler`
- controllers dispatching to MediatR — no direct service calls
- a README explaining when CQRS adds value and when it is overkill

---

### 11 — Result Pattern

**Folder:** `samples/11-result-pattern`

**Concepts demonstrated:**
- returning success or failure without throwing exceptions for expected errors
- `Result<T>` and `Result` types
- chaining operations with `Map` / `Bind`
- the difference between expected failures (domain) and unexpected failures (infrastructure)

**What the sample contains:**
- `Result<T>` record with `IsSuccess`, `Value`, `Error`
- `ProductService.Create` returning `Result<ProductResponse>` instead of throwing
- controller mapping `Result` to appropriate HTTP responses
- comparison: the same flow with exceptions vs. with Result

---

### 12 — Outbox Pattern (Intro)

**Folder:** `samples/12-outbox-pattern`

**Concepts demonstrated:**
- the dual-write problem (saving to DB and sending an event atomically)
- outbox table as a reliable event queue
- background worker reading and publishing outbox messages
- at-least-once delivery guarantee

**What the sample contains:**
- `OutboxMessage` entity persisted in the same transaction as the domain change
- `OutboxPublisher` background service reading unprocessed messages and logging them (simulating a message broker)
- EF Core transaction wrapping domain save + outbox write
- a README explaining the problem this pattern solves and its trade-offs

---

## Structure Rule

Every sample must include:

- `README.md` explaining:
  - what problem the pattern solves
  - how to run (`dotnet run`)
  - what to observe in the output or Swagger
  - when to use and when not to use this pattern
- working code that compiles and runs
- no shared state between samples (each is fully independent)

---

## Technology

- .NET 9
- ASP.NET Core Web API
- EF Core + SQLite (where persistence is needed — no PostgreSQL setup required)
- xUnit + Moq (testing samples)
- Serilog (logging sample)
- FluentValidation (validation sample)
- MediatR (CQRS sample)
- AutoMapper (DTO mapping sample)
