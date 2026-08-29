using Clientes.Domain.Entities;

namespace Clientes.UnitTests.Entities;

public class ClienteTests
{
    [Fact]
    public void DeveCriarClienteComDadosValidos()
    {
        var nome = "João da Silva";
        var cpf = "12345678909";
        var email = "joao@email.com";
        var dataNascimento = new DateTime(1995, 5, 10);

        var cliente = new Cliente(nome, cpf, email, dataNascimento);

        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.Equal(nome, cliente.Nome);
        Assert.Equal(cpf, cliente.Cpf);
        Assert.Equal(email, cliente.Email);
        Assert.Equal(dataNascimento, cliente.DataNascimento);
    }

    [Theory]
    [InlineData("", "12345678909", "joao@email.com", "nome")]
    [InlineData("João da Silva", "", "joao@email.com", "cpf")]
    [InlineData("João da Silva", "12345678909", "", "email")]
    public void DeveRecusarCamposObrigatorios(string nome, string cpf, string email, string campo)
    {
        // Arrange, Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            new Cliente(nome, cpf, email, new DateTime(1995, 5, 10)));

        Assert.Equal(campo, ex.ParamName);
    }

    [Theory]
    [InlineData("12345678900")]  // dígito verificador errado
    [InlineData("11111111111")]  // todos os dígitos iguais
    [InlineData("123456789")]    // menos de 11 dígitos
    public void DeveRecusarCpfInvalido(string cpf)
    {
        // Arrange, Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => CriarCom(cpf));

        Assert.Equal("cpf", ex.ParamName);
        Assert.StartsWith("CPF inválido.", ex.Message);
    }

    [Fact]
    public void DeveArmazenarCpfSomenteComDigitos()
    {
        // Arrange
        var cpfFormatado = "123.456.789-09";

        // Act
        var cliente = CriarCom(cpfFormatado);

        // Assert
        Assert.Equal("12345678909", cliente.Cpf);
    }

    [Theory]
    [InlineData("joao")]
    [InlineData("joao@")]
    [InlineData("joao@localhost")]
    public void DeveRecusarEmailInvalido(string email)
    {
        // Arrange, Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            new Cliente("João da Silva", "12345678909", email, new DateTime(1995, 5, 10)));

        Assert.Equal("email", ex.ParamName);
        Assert.StartsWith("E-mail inválido.", ex.Message);
    }

    private static Cliente CriarCom(string cpf) =>
        new("João da Silva", cpf, "joao@email.com", new DateTime(1995, 5, 10));
}
