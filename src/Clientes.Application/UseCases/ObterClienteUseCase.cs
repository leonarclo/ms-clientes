using Clientes.Application.Abstractions;
using Clientes.Application.Dtos;
using Clientes.Application.Exceptions;

namespace Clientes.Application.UseCases;

public sealed class ObterClienteUseCase(IClienteRepository repositorio)
{
    public async Task<ClienteResponse> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var cliente = await repositorio.ObterPorIdAsync(id, cancellationToken)
            ?? throw new ClienteNaoEncontradoException(id);

        return ClienteResponse.De(cliente);
    }
}
