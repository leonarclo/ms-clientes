using Clientes.Application.Abstractions;
using Clientes.Infrastructure.Messaging;
using Clientes.Infrastructure.Persistence;
using MassTransit;
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

        var rabbit = configuration.GetSection("RabbitMq");

        services.AddMassTransit(configurador =>
        {
            configurador.SetKebabCaseEndpointNameFormatter();

            configurador.UsingRabbitMq((contexto, cfg) =>
            {
                cfg.Host(
                    rabbit["Host"] ?? "localhost",
                    ushort.Parse(rabbit["Port"] ?? "5672"),
                    rabbit["VirtualHost"] ?? "/",
                    host =>
                    {
                        host.Username(rabbit["Username"] ?? "guest");
                        host.Password(rabbit["Password"] ?? "guest");
                    });

                cfg.ConfigureEndpoints(contexto);
            });
        });

        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

        return services;
    }
}
