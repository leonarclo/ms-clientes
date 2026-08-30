using Clientes.Application.Abstractions;
using Clientes.Application.Dtos;
using Clientes.Application.Exceptions;
using Clientes.Domain.Entities;

namespace Clientes.Application.UseCases;

public sealed class CriarClienteUseCase(IClienteRepository repositorio)
{
    public async Task<ClienteResponse> ExecutarAsync(
        CriarClienteRequest request,
        CancellationToken cancellationToken = default)
    {
        var cliente = new Cliente(
            Guid.NewGuid(),
            request.Nome,
            request.Cpf,
            request.Email,
            request.DataNascimento,
            DateTime.UtcNow);

        if (await repositorio.ExisteCpfAsync(cliente.Cpf, cancellationToken))
            throw new CpfJaCadastradoException(cliente.Cpf);

        await repositorio.InserirAsync(cliente, cancellationToken);

        return ClienteResponse.De(cliente);
    }
}
