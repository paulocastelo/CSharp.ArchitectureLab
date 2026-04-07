# 03 — DTOs and Mapping

## What problem this pattern solves

DTOs protect the API contract from domain internals. They let you expose only safe fields and keep internal data private.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5003/swagger`

## What to observe

- `GET /api/products/manual` uses the manual extension mapping
- `GET /api/products/automapper` uses AutoMapper
- `InternalCost` and `InternalNotes` never appear in the response DTO

## When to use

Use DTOs whenever your domain model contains internal, sensitive, or persistence-oriented fields.

## When not to use

For very small internal apps, heavy mapping layers can be overkill if the domain and API contract are nearly identical.
