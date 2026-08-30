namespace Clientes.Application.Dtos;

public sealed record CriarClienteRequest(
    string Nome,
    string Cpf,
    string Email,
    DateTime DataNascimento);
