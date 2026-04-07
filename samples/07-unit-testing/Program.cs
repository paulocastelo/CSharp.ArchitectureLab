using UnitTesting.Sample.Application;
using UnitTesting.Sample.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5007");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddScoped<StockMovementService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
