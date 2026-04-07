# 10 — CQRS Basics

## What problem this pattern solves

CQRS separates reads and writes so that each use case has its own request model and handler.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5010/swagger`

## What to observe

Request flow:

`Controller -> IMediator -> Command/Query Handler -> Response`

The controller only dispatches messages and never talks directly to a service.

## When to use

Use CQRS when a system has many use cases, complex reads, or rapidly evolving business workflows.

## When not to use

For a small CRUD app, CQRS can be unnecessary ceremony and make the codebase noisier than needed.
