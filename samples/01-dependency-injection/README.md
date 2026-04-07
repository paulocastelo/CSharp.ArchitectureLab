# 01 — Dependency Injection

## What problem this pattern solves

Dependency Injection removes hard-coded dependencies and lets the framework construct objects for you. It improves decoupling, testing, and lifecycle management.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5001/swagger`

## What to observe

Call `GET /api/lifetime` and compare the instance IDs:
- `transient1` and `transient2` are different in the same request
- `scoped1` and `scoped2` are equal in the same request
- `singleton` remains the same across all requests

## When to use

- `Transient` for lightweight stateless services
- `Scoped` for request-bound services such as repositories and DbContexts
- `Singleton` for expensive shared services without request state

## When not to use

Avoid `Singleton` when the service depends on mutable request data or non-thread-safe state.
