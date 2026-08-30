using Clientes.Application.Dtos;
using Clientes.Application.Exceptions;
using Clientes.Application.UseCases;
using Clientes.UnitTests.Fakes;

namespace Clientes.UnitTests.UseCases;

public class CriarClienteUseCaseTests
{
    private readonly ClienteRepositoryFake _repositorio = new();

    [Fact]
    public async Task DeveCadastrarClienteComDadosValidos()
    {
        // Arrange
        var useCase = new CriarClienteUseCase(_repositorio);
        var request = NovaRequisicao();

        // Act
        var response = await useCase.ExecutarAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("João da Silva", response.Nome);
        Assert.Single(_repositorio.Inseridos);
    }

    [Fact]
    public async Task DeveRecusarCpfJaCadastrado()
    {
        // Arrange
        var useCase = new CriarClienteUseCase(_repositorio);
        await useCase.ExecutarAsync(NovaRequisicao());

        // Act & Assert
        await Assert.ThrowsAsync<CpfJaCadastradoException>(
            () => useCase.ExecutarAsync(NovaRequisicao()));

        Assert.Single(_repositorio.Inseridos);
    }

    [Fact]
    public async Task DeveDetectarCpfDuplicadoMesmoComFormatacaoDiferente()
    {
        // Arrange
        var useCase = new CriarClienteUseCase(_repositorio);
        await useCase.ExecutarAsync(NovaRequisicao(cpf: "12345678909"));

        // Act & Assert
        await Assert.ThrowsAsync<CpfJaCadastradoException>(
            () => useCase.ExecutarAsync(NovaRequisicao(cpf: "123.456.789-09")));
    }

    private static CriarClienteRequest NovaRequisicao(string cpf = "12345678909") =>
        new("João da Silva", cpf, "joao@email.com", new DateTime(1995, 5, 10));
}
