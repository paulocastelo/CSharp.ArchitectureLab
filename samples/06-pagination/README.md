# 06 — Pagination

## What problem this pattern solves

Pagination avoids returning large datasets at once and gives clients control over how much data to fetch.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5006/swagger`

## What to observe

Call `GET /api/products?page=1&pageSize=10` and inspect the metadata in `PagedResult<T>`.

## When to use

Use offset pagination for standard back-office grids and simple list screens.

## When not to use

For append-only event feeds or extremely large datasets, cursor pagination is often more stable and performant than `Skip` / `Take`.
