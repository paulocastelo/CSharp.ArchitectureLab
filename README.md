# CSharp Architecture Lab

Hands-on **.NET 9** lab with **12 standalone ASP.NET Core Web API samples** that demonstrate core backend architecture and software engineering patterns. Each sample is isolated, uses Swagger, and runs on its own fixed port (`5001` to `5012`).

## What this repository covers

- Dependency Injection and service lifetimes
- Repository abstraction and persistence boundaries
- DTOs, mapping, and API contracts
- Validation, error handling, and pagination
- Unit testing with xUnit + Moq
- JWT authentication basics
- Structured logging and correlation IDs
- CQRS, Result Pattern, and Outbox Pattern

## Samples overview

| # | Sample | Port | Focus |
|---|--------|------|-------|
| [01](samples/01-dependency-injection/) | Dependency Injection | `5001` | Transient, Scoped, and Singleton lifetimes |
| [02](samples/02-repository-pattern/) | Repository Pattern | `5002` | Interface-based persistence abstraction |
| [03](samples/03-dto-mapping/) | DTOs and Mapping | `5003` | Manual mapping vs. AutoMapper |
| [04](samples/04-validation/) | Validation | `5004` | Data Annotations + FluentValidation |
| [05](samples/05-error-handling/) | Error Handling | `5005` | Global middleware + custom exceptions |
| [06](samples/06-pagination/) | Pagination | `5006` | Offset pagination with `PagedResult<T>` |
| [07](samples/07-unit-testing/) | Unit Testing | `5007` | Business rule tests with xUnit + Moq |
| [08](samples/08-auth-basics/) | Auth Basics | `5008` | JWT issuing, validation, and protected routes |
| [09](samples/09-logging/) | Logging | `5009` | Serilog structured logs + correlation ID |
| [10](samples/10-cqrs/) | CQRS Basics | `5010` | Commands, queries, and MediatR |
| [11](samples/11-result-pattern/) | Result Pattern | `5011` | `Result<T>` for expected failures |
| [12](samples/12-outbox-pattern/) | Outbox Pattern | `5012` | Transactional outbox + background publisher |

## Prerequisites

- `.NET SDK 9.0.x`
- Optional: VS Code or Visual Studio 2022

## Quick start

### 1. Restore, build, and test

```bash
dotnet restore CSharp.ArchitectureLab.sln
dotnet build CSharp.ArchitectureLab.sln
dotnet test CSharp.ArchitectureLab.sln --no-build
```

### 2. Run any sample

```bash
dotnet run --project samples/08-auth-basics/AuthBasics.Sample.csproj
# Swagger: http://localhost:5008/swagger
```

## Repository structure

```text
samples/
  01-dependency-injection/
  02-repository-pattern/
  ...
  12-outbox-pattern/
```

> Each folder contains its own `README.md` with scenario details, endpoints, and implementation notes.

## Stack

- .NET 9
- ASP.NET Core Web API
- EF Core + SQLite (when persistence is needed)
- xUnit + Moq
- FluentValidation
- Serilog
- MediatR
- AutoMapper
- JWT Bearer Authentication

## Related repository

[Java.ArchitectureLab](https://github.com/paulocastelo/Java.ArchitectureLab) — the same 12 patterns implemented in Java/Spring Boot.

