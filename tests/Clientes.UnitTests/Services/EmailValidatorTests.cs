using Clientes.Domain.Services;

namespace Clientes.UnitTests.Services;

public class EmailValidatorTests
{
    [Theory]
    [InlineData("joao@email.com")]
    [InlineData("joao.silva+tag@sub.dominio.com.br")]
    [InlineData("  joao@email.com  ")]
    public void DeveAceitarEmailValido(string email)
    {
        Assert.True(EmailValidator.IsValid(email));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("joao")]                    // sem arroba
    [InlineData("joao@")]                   // sem domínio
    [InlineData("@email.com")]              // sem usuário
    [InlineData("joao@localhost")]          // domínio sem ponto
    [InlineData("João <joao@email.com>")]   // formato com nome de exibição
    public void DeveRecusarEmailInvalido(string? email)
    {
        Assert.False(EmailValidator.IsValid(email));
    }
}
