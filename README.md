# CSharp Architecture Lab

Isolated ASP.NET Core samples demonstrating the most important software engineering patterns in idiomatic C#/.NET. Each sample is self-contained and runs independently.

## Samples

| # | Sample | Pattern |
|---|--------|---------|
| [01](samples/01-dependency-injection/) | Dependency Injection | Transient / Scoped / Singleton lifetimes |
| [02](samples/02-repository-pattern/) | Repository Pattern | Interface-based persistence abstraction |
| [03](samples/03-dto-mapping/) | DTOs and Mapping | Manual mapping vs. AutoMapper |
| [04](samples/04-validation/) | Validation | Data Annotations + FluentValidation |
| [05](samples/05-error-handling/) | Error Handling | Global middleware + domain exceptions |
| [06](samples/06-pagination/) | Pagination | Offset-based with `PagedResult<T>` |
| [07](samples/07-unit-testing/) | Unit Testing | xUnit + Moq, business rule coverage |
| [08](samples/08-auth-basics/) | Auth Basics | JWT generation, validation, protected endpoints |
| [09](samples/09-logging/) | Logging | Serilog structured logging + correlation ID |
| [10](samples/10-cqrs/) | CQRS Basics | Commands, queries, MediatR dispatcher |
| [11](samples/11-result-pattern/) | Result Pattern | `Result<T>` without exceptions for expected failures |
| [12](samples/12-outbox-pattern/) | Outbox Pattern | Atomic dual-write with background publisher |

## How to run any sample

```bash
cd samples/01-dependency-injection
dotnet run
# Swagger: http://localhost:5001/swagger
```

## Stack

- .NET 9
- ASP.NET Core Web API
- EF Core + SQLite (where persistence is needed)
- xUnit + Moq (testing)
- Serilog, FluentValidation, MediatR, AutoMapper

## See also

[Java.ArchitectureLab](https://github.com/paulocastelo/Java.ArchitectureLab) — the same 12 patterns implemented in Java/Spring Boot.
