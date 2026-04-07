# 12 — Outbox Pattern (Intro)

## What problem this pattern solves

The Outbox Pattern solves the dual-write problem: saving domain data and publishing an event reliably as part of the same transaction.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5012/swagger`

## What to observe

- `POST /api/products` writes both the product and an outbox message in one transaction
- `GET /api/outbox` shows pending and processed messages
- the background service logs published events every few seconds

## When to use

Use an outbox when a change in your database must also produce an integration event reliably.

## When not to use

For tiny internal apps with no external integrations, the pattern can be unnecessary complexity.
