using Clientes.Api;
using Clientes.Application.UseCases;
using Clientes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<CriarClienteUseCase>();
builder.Services.AddScoped<ObterClienteUseCase>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
