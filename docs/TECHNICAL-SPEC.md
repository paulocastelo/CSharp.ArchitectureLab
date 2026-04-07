# Especificação Técnica — CSharp.ArchitectureLab

Este documento é a especificação de implementação para todos os samples do repositório.
Cada sample é um projeto ASP.NET Core Web API independente, dentro da pasta `samples/`.

---

## Convenções gerais

- Cada sample vive em `samples/NN-nome/` e tem seu próprio `.csproj`
- Nenhum sample depende de outro
- Onde precisar de banco de dados, usar **SQLite** (sem necessidade de instalar nada)
- Cada sample tem um `README.md` com: objetivo, como rodar, o que observar, quando usar
- Todos devem compilar com `dotnet build` e rodar com `dotnet run`
- Swagger habilitado em todos os samples (`http://localhost:PORT/swagger`)
- Porta padrão de cada sample: `5001` + número do sample (5001, 5002, ..., 5012)

---

## 01 — Dependency Injection

**Projeto:** `samples/01-dependency-injection/DependencyInjection.Sample.csproj`

**Dependências NuGet:** apenas `Microsoft.AspNetCore` (built-in)

**Estrutura:**
```
src/
  Services/
    IGreetingService.cs
    TransientGreetingService.cs
    ScopedGreetingService.cs
    SingletonGreetingService.cs
  Controllers/
    LifetimeController.cs
Program.cs
```

**Implementação:**

Cada service implementa `IGreetingService` e expõe `string Greet()` retornando `$"{lifetime}: instance {_instanceId}"` onde `_instanceId = Guid.NewGuid()` é gerado no construtor.

```csharp
public interface IGreetingService { string Greet(); }

public class TransientGreetingService : IGreetingService {
    private readonly Guid _instanceId = Guid.NewGuid();
    public string Greet() => $"Transient: {_instanceId}";
}
// idem para Scoped e Singleton
```

`LifetimeController` injeta os três serviços e retorna todos em um endpoint:

```
GET /api/lifetime
```

Resposta:
```json
{
  "transient1": "Transient: a1b2c3...",
  "transient2": "Transient: d4e5f6...",  // diferente do transient1
  "scoped1": "Scoped: 111aaa...",
  "scoped2": "Scoped: 111aaa...",        // igual ao scoped1 (mesma requisição)
  "singleton": "Singleton: xyz789..."    // sempre igual em todas as chamadas
}
```

Injetar **dois** `Transient` e **dois** `Scoped` no mesmo controller para demonstrar o comportamento visualmente.

Registro em `Program.cs`:
```csharp
builder.Services.AddTransient<IGreetingService, TransientGreetingService>();
// renomear interfaces para ITransientGreeting, IScopedGreeting, ISingletonGreeting
```

---

## 02 — Repository Pattern

**Projeto:** `samples/02-repository-pattern/RepositoryPattern.Sample.csproj`

**Dependências NuGet:** `Microsoft.EntityFrameworkCore.Sqlite`, `Microsoft.EntityFrameworkCore.Design`

**Estrutura:**
```
src/
  Domain/
    Product.cs
  Application/
    IProductRepository.cs
    ProductService.cs
    Contracts/
      CreateProductRequest.cs
      ProductResponse.cs
  Infrastructure/
    InMemoryProductRepository.cs
    EfCoreProductRepository.cs
    AppDbContext.cs
  Controllers/
    ProductsController.cs
Program.cs
```

**`IProductRepository`:**
```csharp
public interface IProductRepository {
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Product> AddAsync(Product product, CancellationToken ct);
}
```

`InMemoryProductRepository` usa `List<Product>` estático.
`EfCoreProductRepository` usa `AppDbContext` com SQLite.

`Program.cs` deve ter comentário explicando como trocar:
```csharp
// Swap implementation here — no other code changes needed:
builder.Services.AddScoped<IProductRepository, InMemoryProductRepository>();
// builder.Services.AddScoped<IProductRepository, EfCoreProductRepository>();
```

Endpoints:
```
GET  /api/products
GET  /api/products/{id}
POST /api/products
```

---

## 03 — DTOs and Mapping

**Projeto:** `samples/03-dto-mapping/DtoMapping.Sample.csproj`

**Dependências NuGet:** `AutoMapper`

**Estrutura:**
```
src/
  Domain/
    Product.cs          // contém: Id, Name, Sku, InternalCost, PasswordHash (simulado)
  Application/
    Contracts/
      CreateProductRequest.cs
      ProductResponse.cs   // não expõe InternalCost nem PasswordHash
    Mappings/
      ProductProfile.cs    // AutoMapper profile
    Extensions/
      ProductMappingExtensions.cs  // mapeamento manual como extensão
    ProductService.cs
  Controllers/
    ProductsController.cs
Program.cs
```

`Product` domain entity:
```csharp
public class Product {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Sku { get; set; }
    public decimal InternalCost { get; set; }  // não deve vazar para a API
    public string InternalNotes { get; set; }  // não deve vazar para a API
}
```

`ProductResponse`:
```csharp
public record ProductResponse(Guid Id, string Name, string Sku);
```

Dois endpoints mostrando os dois métodos:
```
GET /api/products/manual       // usa ProductMappingExtensions
GET /api/products/automapper   // usa AutoMapper
```

O `README.md` deve explicar quando usar cada abordagem.

---

## 04 — Validation

**Projeto:** `samples/04-validation/Validation.Sample.csproj`

**Dependências NuGet:** `FluentValidation.AspNetCore`

**Estrutura:**
```
src/
  Contracts/
    CreateProductRequest.cs     // validado com Data Annotations
    RegisterUserRequest.cs      // validado com FluentValidation
  Validators/
    RegisterUserRequestValidator.cs
  Filters/
    ValidationFilter.cs
  Controllers/
    ProductsController.cs
    UsersController.cs
Program.cs
```

`CreateProductRequest` com Data Annotations:
```csharp
public class CreateProductRequest {
    [Required] [MaxLength(100)] public string Name { get; set; }
    [Required] [MaxLength(50)]  public string Sku { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
    public decimal UnitPrice { get; set; }
}
```

`RegisterUserRequestValidator` com FluentValidation:
```csharp
public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest> {
    public RegisterUserRequestValidator() {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
    }
}
```

Resposta padronizada de erro de validação (`400 Bad Request`):
```json
{
  "type": "validation_error",
  "errors": {
    "email": ["'Email' is not a valid email address."],
    "password": ["Password must contain at least one uppercase letter."]
  }
}
```

Implementar `ValidationFilter` como `IActionFilter` para centralizar esse formato.

---

## 05 — Error Handling

**Projeto:** `samples/05-error-handling/ErrorHandling.Sample.csproj`

**Dependências NuGet:** nenhuma extra

**Estrutura:**
```
src/
  Exceptions/
    NotFoundException.cs
    ConflictException.cs
    BusinessRuleException.cs
  Middleware/
    ExceptionHandlingMiddleware.cs
  Controllers/
    DemoController.cs
Program.cs
```

Exceções do domínio:
```csharp
public class NotFoundException(string detail) : Exception(detail);
public class ConflictException(string detail) : Exception(detail);
public class BusinessRuleException(string detail) : Exception(detail);
```

`ExceptionHandlingMiddleware` deve mapear:
- `NotFoundException` → `404`
- `ConflictException` → `409`
- `BusinessRuleException` → `422`
- qualquer outra → `500` com mensagem genérica (sem vazar stack trace)

Formato de resposta:
```json
{
  "type": "not_found",
  "detail": "Product with id 'abc-123' was not found.",
  "traceId": "0HN1234567890"
}
```

`DemoController` deve ter endpoints que intencionalmente lançam cada tipo de exceção:
```
GET /api/demo/not-found
GET /api/demo/conflict
GET /api/demo/business-rule
GET /api/demo/unhandled
```

---

## 06 — Pagination

**Projeto:** `samples/06-pagination/Pagination.Sample.csproj`

**Dependências NuGet:** `Microsoft.EntityFrameworkCore.Sqlite`

**Estrutura:**
```
src/
  Common/
    PagedResult.cs
    PaginationParams.cs
  Domain/
    Product.cs
  Infrastructure/
    AppDbContext.cs
    DataSeeder.cs       // insere 100 produtos no SQLite na inicialização
  Application/
    ProductService.cs
  Controllers/
    ProductsController.cs
Program.cs
```

`PagedResult<T>`:
```csharp
public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount) {
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
```

`PaginationParams`:
```csharp
public record PaginationParams(
    [Range(1, int.MaxValue)] int Page = 1,
    [Range(1, 100)] int PageSize = 10);
```

Endpoint:
```
GET /api/products?page=1&pageSize=10
```

EF Core:
```csharp
var total = await query.CountAsync(ct);
var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
```

O `README.md` deve incluir uma nota sobre cursor pagination com exemplo de quando preferir.

---

## 07 — Unit Testing

**Projeto:** `samples/07-unit-testing/UnitTesting.Sample.csproj`

**Dependências NuGet:** `xunit`, `Moq`, `Microsoft.NET.Test.Sdk`, `xunit.runner.visualstudio`

**Estrutura:**
```
src/
  Domain/
    Product.cs
    StockMovement.cs
    StockMovementType.cs
  Application/
    IProductRepository.cs
    StockMovementService.cs
    Contracts/
      CreateMovementRequest.cs
tests/
  StockMovementServiceTests.cs
```

`StockMovementService.CreateAsync` deve:
1. Buscar produto por id (`IProductRepository.GetByIdAsync`)
2. Validar quantidade > 0
3. Se EXIT: validar saldo suficiente
4. Atualizar saldo
5. Persistir movimento

Testes obrigatórios:
```csharp
[Fact] void CreateEntry_IncreasesBalance()
[Fact] void CreateExit_DecreasesBalance()
[Fact] void CreateExit_WithInsufficientBalance_ThrowsException()
[Fact] void CreateExit_WithExactBalance_Succeeds()
[Fact] void Create_WithInvalidQuantity_ThrowsException()
[Fact] void Create_WithUnknownProduct_ThrowsNotFoundException()
```

Todos os testes com Arrange/Act/Assert bem delimitados por comentários.

---

## 08 — Auth Basics (JWT)

**Projeto:** `samples/08-auth-basics/AuthBasics.Sample.csproj`

**Dependências NuGet:** `Microsoft.AspNetCore.Authentication.JwtBearer`, `BCrypt.Net-Next`

**Estrutura:**
```
src/
  Domain/
    AppUser.cs
  Application/
    IUserRepository.cs
    AuthService.cs
    JwtService.cs
    Contracts/
      RegisterRequest.cs
      LoginRequest.cs
      AuthResponse.cs
  Infrastructure/
    InMemoryUserRepository.cs
  Controllers/
    AuthController.cs
    ProfileController.cs
Program.cs
```

`JwtService`:
```csharp
public class JwtService {
    string GenerateToken(AppUser user);  // retorna JWT com claims: sub, userId, email, exp
    bool ValidateToken(string token);
    ClaimsPrincipal? GetPrincipal(string token);
}
```

`AuthService`:
```csharp
Task<UserProfileResponse> RegisterAsync(RegisterRequest request);
Task<AuthResponse> LoginAsync(LoginRequest request);
```

Endpoints:
```
POST /api/auth/register   → 201 Created → UserProfileResponse
POST /api/auth/login      → 200 OK      → AuthResponse { accessToken, expiresAtUtc, user }
GET  /api/profile         → 200 OK      → { id, email, fullName }  [Authorize]
```

Swagger configurado com `SecurityDefinition` para Bearer token.

---

## 09 — Logging

**Projeto:** `samples/09-logging/Logging.Sample.csproj`

**Dependências NuGet:** `Serilog.AspNetCore`, `Serilog.Sinks.Console`, `Serilog.Sinks.File`

**Estrutura:**
```
src/
  Middleware/
    RequestLoggingMiddleware.cs
    CorrelationIdMiddleware.cs
  Services/
    ProductService.cs      // demonstra uso de ILogger em serviço
  Controllers/
    ProductsController.cs
Program.cs
```

Serilog configurado em `Program.cs`:
```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();
```

`CorrelationIdMiddleware`:
- lê header `X-Correlation-Id` ou gera um novo `Guid`
- adiciona ao `HttpContext.Items` e ao `Response.Headers`
- enriquece o log context com `LogContext.PushProperty("CorrelationId", ...)`

`RequestLoggingMiddleware`:
- loga `{Method} {Path} → {StatusCode} in {ElapsedMs}ms` em `Information`
- loga body de erros (status >= 400) em `Warning`

`ProductService` deve demonstrar os 4 níveis:
```csharp
_logger.LogDebug("Fetching product {ProductId}", id);
_logger.LogInformation("Product {ProductId} retrieved successfully", id);
_logger.LogWarning("Product {ProductId} is inactive", id);
_logger.LogError(ex, "Failed to retrieve product {ProductId}", id);
```

---

## 10 — CQRS Basics

**Projeto:** `samples/10-cqrs/Cqrs.Sample.csproj`

**Dependências NuGet:** `MediatR`

**Estrutura:**
```
src/
  Domain/
    Product.cs
  Application/
    Commands/
      CreateProductCommand.cs
      CreateProductCommandHandler.cs
    Queries/
      GetProductByIdQuery.cs
      GetProductByIdQueryHandler.cs
      GetProductsQuery.cs
      GetProductsQueryHandler.cs
    Contracts/
      CreateProductRequest.cs
      ProductResponse.cs
  Infrastructure/
    InMemoryProductRepository.cs
  Controllers/
    ProductsController.cs
Program.cs
```

Commands:
```csharp
public record CreateProductCommand(string Name, string Sku, decimal UnitPrice) : IRequest<ProductResponse>;
```

Queries:
```csharp
public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;
public record GetProductsQuery(int Page, int PageSize) : IRequest<PagedResult<ProductResponse>>;
```

Controller despacha para MediatR — **nenhuma injeção de service diretamente**:
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateProductRequest request) {
    var command = new CreateProductCommand(request.Name, request.Sku, request.UnitPrice);
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
}
```

O `README.md` deve incluir diagrama em texto explicando o fluxo Request → Handler → Response e quando CQRS adiciona e quando não adiciona valor.

---

## 11 — Result Pattern

**Projeto:** `samples/11-result-pattern/ResultPattern.Sample.csproj`

**Dependências NuGet:** nenhuma extra

**Estrutura:**
```
src/
  Common/
    Result.cs
    ResultExtensions.cs
  Domain/
    Product.cs
  Application/
    ProductService.cs
  Controllers/
    ProductsController.cs
Program.cs
```

`Result<T>`:
```csharp
public class Result<T> {
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public string? ErrorCode { get; }

    private Result(T value) { IsSuccess = true; Value = value; }
    private Result(string error, string errorCode) { IsSuccess = false; Error = error; ErrorCode = errorCode; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error, string errorCode) => new(error, errorCode);
}
```

`ProductService.CreateAsync` retorna `Result<ProductResponse>` — sem lançar exceção para erros esperados:
```csharp
if (await _repository.ExistsBySkuAsync(request.Sku))
    return Result<ProductResponse>.Failure("SKU already exists.", "sku_conflict");
```

Controller mapeia `Result` para HTTP:
```csharp
if (!result.IsSuccess) {
    return result.ErrorCode switch {
        "sku_conflict" => Conflict(new { detail = result.Error }),
        "not_found"    => NotFound(new { detail = result.Error }),
        _              => BadRequest(new { detail = result.Error }),
    };
}
return Ok(result.Value);
```

O `README.md` deve incluir a comparação lado a lado: mesmo fluxo com exceções vs. com Result.

---

## 12 — Outbox Pattern

**Projeto:** `samples/12-outbox-pattern/OutboxPattern.Sample.csproj`

**Dependências NuGet:** `Microsoft.EntityFrameworkCore.Sqlite`

**Estrutura:**
```
src/
  Domain/
    Product.cs
  Infrastructure/
    AppDbContext.cs
    OutboxMessage.cs
    OutboxPublisher.cs    // BackgroundService
  Application/
    ProductService.cs
  Controllers/
    ProductsController.cs
Program.cs
```

`OutboxMessage`:
```csharp
[Table("outbox_messages")]
public class OutboxMessage {
    public Guid Id { get; set; }
    public string EventType { get; set; }   // ex.: "ProductCreated"
    public string Payload { get; set; }     // JSON serializado
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ProcessedAtUtc { get; set; }
}
```

`ProductService.CreateAsync`:
```csharp
// Em uma única transação:
await using var transaction = await _dbContext.Database.BeginTransactionAsync();
_dbContext.Products.Add(product);
_dbContext.OutboxMessages.Add(new OutboxMessage {
    Id = Guid.NewGuid(),
    EventType = "ProductCreated",
    Payload = JsonSerializer.Serialize(new { product.Id, product.Name }),
    CreatedAtUtc = DateTime.UtcNow,
});
await _dbContext.SaveChangesAsync();
await transaction.CommitAsync();
```

`OutboxPublisher` (`BackgroundService`):
```csharp
// A cada 5 segundos:
var pending = await _dbContext.OutboxMessages
    .Where(m => m.ProcessedAtUtc == null)
    .Take(10)
    .ToListAsync();

foreach (var message in pending) {
    _logger.LogInformation("Publishing {EventType}: {Payload}", message.EventType, message.Payload);
    message.ProcessedAtUtc = DateTime.UtcNow;
}
await _dbContext.SaveChangesAsync();
```

Endpoint para criar produto e observar o outbox sendo processado nos logs:
```
POST /api/products
GET  /api/outbox   → lista mensagens com status (processado/pendente)
```

---

## README raiz do repositório

O `README.md` da raiz deve listar todos os 12 samples com uma linha de descrição e o link para a pasta de cada um. Deve também ter uma seção "How to run any sample":

```bash
cd samples/01-dependency-injection
dotnet run
# Swagger: http://localhost:5001/swagger
```

---

## Critério de conclusão

A implementação está concluída quando:

1. Todos os 12 samples compilam com `dotnet build`
2. Todos os samples sobem com `dotnet run`
3. Os testes do sample 07 passam com `dotnet test`
4. Cada sample tem um `README.md` com objetivo, como rodar e o que observar
5. O `README.md` raiz lista todos os samples com descrições
