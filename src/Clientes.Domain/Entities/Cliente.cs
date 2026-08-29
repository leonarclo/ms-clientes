using Clientes.Domain.Services;

namespace Clientes.Domain.Entities;

public class Cliente
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public DateTime DataCadastro { get; private set; }

    public Cliente(
        string nome,
        string cpf,
        string email,
        DateTime dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF é obrigatório.", nameof(cpf));

        if (!CpfValidator.IsValid(cpf))
            throw new ArgumentException("CPF inválido.", nameof(cpf));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("E-mail é obrigatório.", nameof(email));

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        Cpf = CpfValidator.Normalizar(cpf);
        Email = email.Trim();
        DataNascimento = dataNascimento;
        DataCadastro = DateTime.UtcNow;
    }
}
