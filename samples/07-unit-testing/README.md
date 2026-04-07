# 07 — Unit Testing

## What problem this pattern solves

Unit tests verify business rules in isolation, without hitting a real database or network.

## How to run

```bash
dotnet run
dotnet test tests/UnitTesting.Sample.Tests.csproj
```

Swagger: `http://localhost:5007/swagger`

## What to observe

The tests cover stock entry, stock exit, insufficient balance, exact balance, invalid quantity, and unknown product scenarios.

## When to use

Use unit tests for domain rules and application services where the logic must stay stable during refactors.

## When not to use

Do not use unit tests alone to prove that infrastructure integration works; add integration tests when you need to validate databases or HTTP flows.
