using Clientes.Domain.Entities;

namespace Clientes.Application.Abstractions;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExisteCpfAsync(string cpf, CancellationToken cancellationToken = default);

    Task InserirAsync(Cliente cliente, CancellationToken cancellationToken = default);
}
