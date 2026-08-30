using Clientes.Application.Abstractions;
using Clientes.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clientes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ClientesDb")
            ?? throw new InvalidOperationException("Connection string 'ClientesDb' não configurada.");

        services.AddScoped<IClienteRepository>(_ => new ClienteRepository(connectionString));

        return services;
    }
}
