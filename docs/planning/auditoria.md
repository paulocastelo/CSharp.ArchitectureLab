# Auditoria — CSharp.ArchitectureLab

Data: 2026-04-06

## Resultado geral

Implementação aprovada com **2 desvios encontrados**, ambos no sample 11 (Result Pattern). Os demais 11 samples estão em conformidade com o spec.

---

## Checklist por sample

### 01 — Dependency Injection ✅
- Interfaces separadas por lifetime (`ITransientGreetingService`, `IScopedGreetingService`, `ISingletonGreetingService`) — correto
- Controller injeta dois transients e dois scopeds para demonstrar o comportamento visualmente — correto
- Port 5001, registro correto no `Program.cs` — correto

### 02 — Repository Pattern ✅
- Interface `IProductRepository` com implementações `EfCoreProductRepository` (EF Core + SQLite) e `InMemoryProductRepository` — correto
- `ProductService` depende apenas da interface — correto
- `AppDbContext` com `DbSet<Product>` — correto

### 03 — DTO Mapping ✅
- Mapeamento manual via `ProductMappingExtensions` — correto
- Mapeamento com AutoMapper via `ProductProfile` — correto
- Dois endpoints demonstrando as duas abordagens — correto

### 04 — Validation ✅
- `CreateProductRequest` com Data Annotations — correto
- `RegisterUserRequestValidator` com FluentValidation: validações de Email, Password (uppercase + dígito), FullName MaxLength — correto
- `ValidationFilter` integrado ao pipeline — correto

### 05 — Error Handling ✅
- `ExceptionHandlingMiddleware` com switch expression mapeando NotFoundException→404, ConflictException→409, BusinessRuleException→422, outros→500 — correto
- Payload JSON retorna `type`, `detail`, `traceId` — correto
- `DemoController` com 4 endpoints de demonstração — correto

### 06 — Pagination ✅
- `PagedResult<T>` como `record` com `TotalPages`, `HasNextPage`, `HasPreviousPage` calculados — correto
- `PaginationParams` com `Page` e `PageSize` — correto
- `DataSeeder` para popular dados de demonstração — correto

### 07 — Unit Testing ✅
- 6 casos de teste cobrindo: entry aumenta saldo, exit diminui, saldo insuficiente lança `InvalidOperationException`, saída com saldo exato funciona, quantidade inválida lança `ArgumentOutOfRangeException`, produto desconhecido lança `KeyNotFoundException` — correto
- Moq configurado com `GetByIdAsync`, `SaveAsync`, `AddMovementAsync` — correto
- Estrutura AAA em todos os testes — correto

### 08 — Auth Basics ✅
- `JwtService.GenerateToken` emite token com claims `sub`, `userId`, `email`, `fullName`, expiração de 1h — correto
- `ClockSkew = TimeSpan.Zero` na validação — correto
- `ProfileController` protegido com `[Authorize]` e lê claims do token — correto
- Port 5008 — correto

### 09 — Logging ✅
- Serilog configurado com `JsonFormatter` no console + arquivo rolling diário — correto
- `CorrelationIdMiddleware` lê `X-Correlation-Id` do request (ou gera um novo) e faz push para `LogContext` via `Serilog.Context.LogContext.PushProperty` — correto
- Header `X-Correlation-Id` propagado na response — correto
- Port 5009 — correto

### 10 — CQRS ✅
- MediatR registrado via `RegisterServicesFromAssemblyContaining<CreateProductCommand>()` — correto
- Commands e Queries em pastas separadas — correto
- Controller despacha apenas para `IMediator`, sem lógica de negócio — correto
- Port 5010 — correto

### 11 — Result Pattern ⚠️ DESVIOS

#### Desvio 1 — `Result<T>` não é `sealed`
**Arquivo:** `samples/11-result-pattern/src/Common/Result.cs:3`

```csharp
// atual (incorreto)
public class Result<T>

// esperado
public sealed class Result<T>
```

`Result<T>` é um value object de uso interno — herança não é desejada e a classe deve ser `sealed`.

#### Desvio 2 — Método `CreateAsync` é síncrono
**Arquivo:** `samples/11-result-pattern/src/Application/ProductService.cs:13`

```csharp
// atual (incorreto)
public Result<ProductResponse> CreateAsync(CreateProductRequest request)

// esperado
public Result<ProductResponse> Create(CreateProductRequest request)
```

O método não é assíncrono (não retorna `Task`). O sufixo `Async` viola a convenção .NET de nomenclatura e é inconsistente com o restante da codebase. O controller já o chama corretamente sem `await` — basta remover o sufixo do nome.

### 12 — Outbox Pattern ✅
- `ProductService.CreateAsync` usa transação explícita: `BeginTransactionAsync` → adiciona `Product` + `OutboxMessage` → `SaveChangesAsync` → `CommitAsync` — atomicidade demonstrada corretamente
- `OutboxPublisher` como `BackgroundService` com polling a cada 5s, processa até 10 mensagens pendentes, marca `ProcessedAtUtc` — correto
- Port 5012 — correto

---

## Resumo dos ajustes necessários

| # | Sample | Arquivo | Ajuste |
|---|--------|---------|--------|
| 1 | 11-result-pattern | `src/Common/Result.cs:3` | Adicionar `sealed` na declaração da classe |
| 2 | 11-result-pattern | `src/Application/ProductService.cs:13` | Renomear `CreateAsync` para `Create` |

---

## Ação

Ajustes pontuais em dois arquivos do sample 11. Todos os demais samples estão aprovados.
