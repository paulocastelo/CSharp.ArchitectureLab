# 11 — Result Pattern

## What problem this pattern solves

The Result Pattern represents expected failures explicitly without throwing exceptions for normal business outcomes.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5011/swagger`

## What to observe

`ProductService.CreateAsync` returns `Result<ProductResponse>`. The controller maps the result to `200`, `409`, or `400` depending on the failure code.

## When to use

Use the Result Pattern for expected domain failures such as conflicts, validation issues, or missing state.

## When not to use

Unexpected infrastructure failures should still be exceptions; Result is not a replacement for proper error handling middleware.
