namespace Clientes.Application.Exceptions;

public sealed class ClienteNaoEncontradoException(Guid id)
    : Exception($"Cliente {id} não encontrado.")
{
    public Guid Id { get; } = id;
}
