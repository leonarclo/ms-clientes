namespace Clientes.Application.Exceptions;

public sealed class CpfJaCadastradoException(string cpf)
    : Exception($"Já existe um cliente cadastrado com o CPF {cpf}.")
{
    public string Cpf { get; } = cpf;
}
