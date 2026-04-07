# 02 — Repository Pattern

## What problem this pattern solves

The Repository Pattern separates domain logic from persistence details. The service depends on `IProductRepository`, not on EF Core or SQLite directly.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5002/swagger`

## What to observe

- `ProductService` works with the interface only
- `InMemoryProductRepository` is useful for demos and testing
- `EfCoreProductRepository` can be enabled in `Program.cs` without changing the service or controller

## When to use

Use it when you want to hide persistence details and keep application code focused on business rules.

## When not to use

Avoid overly abstract repositories for simple CRUD systems where EF Core already gives enough abstraction.
