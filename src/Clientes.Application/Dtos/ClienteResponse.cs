using Clientes.Domain.Entities;

namespace Clientes.Application.Dtos;

public sealed record ClienteResponse(
    Guid Id,
    string Nome,
    string Cpf,
    string Email,
    DateTime DataNascimento,
    DateTime DataCadastro)
{
    public static ClienteResponse De(Cliente cliente) =>
        new(cliente.Id,
            cliente.Nome,
            cliente.Cpf,
            cliente.Email,
            cliente.DataNascimento,
            cliente.DataCadastro);
}
