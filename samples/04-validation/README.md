# 04 — Validation

## What problem this pattern solves

Validation protects the API boundary. It stops invalid input early and returns a predictable error format to clients.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5004/swagger`

## What to observe

- `POST /api/products` uses Data Annotations
- `POST /api/users/register` uses FluentValidation with custom password and email rules
- invalid payloads return a standardized `validation_error` response body

## When to use

Use validation at the API boundary for all user input, especially when multiple clients depend on consistent error responses.

## When not to use

Do not rely only on UI validation; server-side validation is still required even when the frontend already validates forms.
