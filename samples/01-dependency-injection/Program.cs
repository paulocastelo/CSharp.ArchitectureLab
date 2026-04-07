using DependencyInjection.Sample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5001");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITransientGreetingService, TransientGreetingService>();
builder.Services.AddScoped<IScopedGreetingService, ScopedGreetingService>();
builder.Services.AddSingleton<ISingletonGreetingService, SingletonGreetingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
