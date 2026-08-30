using Clientes.Application.Abstractions;
using MassTransit;

namespace Clientes.Infrastructure.Messaging;

public sealed class MassTransitEventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public Task PublicarAsync<T>(T evento, CancellationToken cancellationToken = default)
        where T : class =>
        publishEndpoint.Publish(evento, cancellationToken);
}
