using Clientes.Domain.Entities;

namespace Clientes.UnitTests.Entities;

public class ClienteTests
{
    private static readonly DateTime Nascimento = new(1995, 5, 10);
    private static readonly DateTime Cadastro = new(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc);

    [Fact]
    public void DeveCriarClienteComDadosValidos()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var cliente = new Cliente(
            id, "João da Silva", "12345678909", "joao@email.com", Nascimento, Cadastro);

        // Assert
        Assert.Equal(id, cliente.Id);
        Assert.Equal("João da Silva", cliente.Nome);
        Assert.Equal("12345678909", cliente.Cpf);
        Assert.Equal("joao@email.com", cliente.Email);
        Assert.Equal(Nascimento, cliente.DataNascimento);
        Assert.Equal(Cadastro, cliente.DataCadastro);
    }

    [Fact]
    public void DeveRecusarIdVazio()
    {
        var ex = Assert.Throws<ArgumentException>(() => Criar(id: Guid.Empty));

        Assert.Equal("id", ex.ParamName);
    }

    [Theory]
    [InlineData("", "12345678909", "joao@email.com", "nome")]
    [InlineData("João da Silva", "", "joao@email.com", "cpf")]
    [InlineData("João da Silva", "12345678909", "", "email")]
    public void DeveRecusarCamposObrigatorios(string nome, string cpf, string email, string campo)
    {
        var ex = Assert.Throws<ArgumentException>(() => Criar(nome: nome, cpf: cpf, email: email));

        Assert.Equal(campo, ex.ParamName);
    }

    [Theory]
    [InlineData("12345678900")]  // dígito verificador errado
    [InlineData("11111111111")]  // todos os dígitos iguais
    [InlineData("123456789")]    // menos de 11 dígitos
    public void DeveRecusarCpfInvalido(string cpf)
    {
        var ex = Assert.Throws<ArgumentException>(() => Criar(cpf: cpf));

        Assert.Equal("cpf", ex.ParamName);
        Assert.StartsWith("CPF inválido.", ex.Message);
    }

    [Theory]
    [InlineData("joao")]
    [InlineData("joao@")]
    [InlineData("joao@localhost")]
    public void DeveRecusarEmailInvalido(string email)
    {
        var ex = Assert.Throws<ArgumentException>(() => Criar(email: email));

        Assert.Equal("email", ex.ParamName);
        Assert.StartsWith("E-mail inválido.", ex.Message);
    }

    [Fact]
    public void DeveArmazenarCpfSomenteComDigitos()
    {
        var cliente = Criar(cpf: "123.456.789-09");

        // Assert
        Assert.Equal("12345678909", cliente.Cpf);
    }

    private static Cliente Criar(
        Guid? id = null,
        string nome = "João da Silva",
        string cpf = "12345678909",
        string email = "joao@email.com") =>
        new(id ?? Guid.NewGuid(), nome, cpf, email, Nascimento, Cadastro);
}
