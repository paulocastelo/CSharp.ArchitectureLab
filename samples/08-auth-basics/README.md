# 08 — Auth Basics (JWT)

## What problem this pattern solves

JWT authentication protects API endpoints and lets the server identify the caller without keeping session state.

## How to run

```bash
dotnet run
```

Swagger: `http://localhost:5008/swagger`

## What to observe

- `POST /api/auth/register` creates a user with a hashed password
- `POST /api/auth/login` returns a JWT bearer token
- `GET /api/profile` requires authorization and reads claims from the token

## When to use

Use JWT for stateless APIs and mobile/web clients that need bearer-token authentication.

## When not to use

Do not use JWT blindly for complex session management scenarios when cookie-based auth or a full refresh-token flow is a better fit.
