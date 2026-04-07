# 05 — Error Handling

## What problem this pattern solves

A global exception middleware keeps API error responses consistent and prevents stack traces or sensitive details from leaking to clients.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5005/swagger`

## What to observe

Try the endpoints under `/api/demo/*` and see how each exception maps to a specific HTTP status and error envelope.

## When to use

Use global error handling in all production APIs to standardize failures and centralize logging.

## When not to use

Do not throw exceptions for routine validation errors or expected user mistakes when a normal result or validation response is more appropriate.
