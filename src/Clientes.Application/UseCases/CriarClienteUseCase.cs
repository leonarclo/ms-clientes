using Clientes.Application.Abstractions;
using Clientes.Application.Dtos;
using Clientes.Application.Exceptions;
using Clientes.Domain.Entities;
using Contracts.Events;

namespace Clientes.Application.UseCases;

public sealed class CriarClienteUseCase(
    IClienteRepository repositorio,
    IEventPublisher publicador)
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

        await publicador.PublicarAsync(new ClienteCadastrado(
            EventId: Guid.NewGuid(),
            ClienteId: cliente.Id,
            Nome: cliente.Nome,
            Cpf: cliente.Cpf,
            Email: cliente.Email,
            DataNascimento: DateOnly.FromDateTime(cliente.DataNascimento),
            OccurredAt: DateTimeOffset.UtcNow), cancellationToken);

        return ClienteResponse.De(cliente);
    }
}
