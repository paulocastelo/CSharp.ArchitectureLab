using FluentValidation;
using Validation.Sample.Contracts;
using Validation.Sample.Filters;
using Validation.Sample.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5004");

builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
