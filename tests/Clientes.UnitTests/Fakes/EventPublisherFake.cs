using Clientes.Application.Abstractions;

namespace Clientes.UnitTests.Fakes;

public sealed class EventPublisherFake : IEventPublisher
{
    private readonly List<object> _publicados = [];

    public IReadOnlyList<object> Publicados => _publicados;

    public Task PublicarAsync<T>(T evento, CancellationToken cancellationToken = default)
        where T : class
    {
        _publicados.Add(evento);
        return Task.CompletedTask;
    }
}
