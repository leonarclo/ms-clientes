namespace Clientes.Application.Abstractions;

public interface IEventPublisher
{
    Task PublicarAsync<T>(T evento, CancellationToken cancellationToken = default)
        where T : class;
}
