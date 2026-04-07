# 09 — Logging

## What problem this pattern solves

Structured logging makes it easier to trace requests, diagnose errors, and correlate events across an API.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5009/swagger`

## What to observe

- logs are written to console and rolling files
- every request receives an `X-Correlation-Id`
- service methods demonstrate `Debug`, `Information`, `Warning`, and `Error`

## When to use

Use structured logging in production APIs where observability and supportability matter.

## When not to use

Avoid ad-hoc `Console.WriteLine` logging in real applications because it lacks structure, context, and level control.
