using Clientes.Application.Abstractions;
using Clientes.Domain.Entities;

namespace Clientes.UnitTests.Fakes;

/// <summary>
/// Repositório em memória. Existe para provar o caso de uso sem SQL Server,
/// que é justamente o motivo de IClienteRepository ser uma interface.
/// </summary>
public sealed class ClienteRepositoryFake : IClienteRepository
{
    private readonly List<Cliente> _clientes = [];

    public IReadOnlyList<Cliente> Inseridos => _clientes;

    public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_clientes.FirstOrDefault(c => c.Id == id));

    public Task<bool> ExisteCpfAsync(string cpf, CancellationToken cancellationToken = default) =>
        Task.FromResult(_clientes.Any(c => c.Cpf == cpf));

    public Task InserirAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        _clientes.Add(cliente);
        return Task.CompletedTask;
    }
}
