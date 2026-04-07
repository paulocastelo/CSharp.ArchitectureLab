using AutoMapper;
using DtoMapping.Sample.Application;
using DtoMapping.Sample.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5003");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMapper>(_ =>
{
    var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());
    return configuration.CreateMapper();
});
builder.Services.AddSingleton<ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
