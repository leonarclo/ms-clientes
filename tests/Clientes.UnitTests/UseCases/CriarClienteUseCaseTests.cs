using Clientes.Application.Dtos;
using Clientes.Application.Exceptions;
using Clientes.Application.UseCases;
using Clientes.UnitTests.Fakes;
using Contracts.Events;

namespace Clientes.UnitTests.UseCases;

public class CriarClienteUseCaseTests
{
    private readonly ClienteRepositoryFake _repositorio = new();
    private readonly EventPublisherFake _publicador = new();

    private CriarClienteUseCase CriarUseCase() => new(_repositorio, _publicador);

    [Fact]
    public async Task DeveCadastrarClienteComDadosValidos()
    {
        // Arrange
        var useCase = CriarUseCase();

        // Act
        var response = await useCase.ExecutarAsync(NovaRequisicao());

        // Assert
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("João da Silva", response.Nome);
        Assert.Single(_repositorio.Inseridos);
    }

    [Fact]
    public async Task DevePublicarEventoClienteCadastrado()
    {
        // Arrange
        var useCase = CriarUseCase();

        // Act
        var response = await useCase.ExecutarAsync(NovaRequisicao(cpf: "123.456.789-09"));

        // Assert
        var evento = Assert.IsType<ClienteCadastrado>(Assert.Single(_publicador.Publicados));

        Assert.Equal(response.Id, evento.ClienteId);
        Assert.NotEqual(Guid.Empty, evento.EventId);
        Assert.Equal("12345678909", evento.Cpf);
        Assert.Equal(new DateOnly(1995, 5, 10), evento.DataNascimento);
    }

    [Fact]
    public async Task DeveRecusarCpfJaCadastrado()
    {
        // Arrange
        var useCase = CriarUseCase();
        await useCase.ExecutarAsync(NovaRequisicao());

        // Act & Assert
        await Assert.ThrowsAsync<CpfJaCadastradoException>(
            () => useCase.ExecutarAsync(NovaRequisicao()));

        Assert.Single(_repositorio.Inseridos);
    }

    [Fact]
    public async Task NaoDevePublicarEventoQuandoCpfDuplicado()
    {
        // Arrange
        var useCase = CriarUseCase();
        await useCase.ExecutarAsync(NovaRequisicao());

        // Act
        await Assert.ThrowsAsync<CpfJaCadastradoException>(
            () => useCase.ExecutarAsync(NovaRequisicao()));

        // Assert
        Assert.Single(_publicador.Publicados);
    }

    [Fact]
    public async Task DeveDetectarCpfDuplicadoMesmoComFormatacaoDiferente()
    {
        // Arrange
        var useCase = CriarUseCase();
        await useCase.ExecutarAsync(NovaRequisicao(cpf: "12345678909"));

        // Act & Assert
        await Assert.ThrowsAsync<CpfJaCadastradoException>(
            () => useCase.ExecutarAsync(NovaRequisicao(cpf: "123.456.789-09")));
    }

    private static CriarClienteRequest NovaRequisicao(string cpf = "12345678909") =>
        new("João da Silva", cpf, "joao@email.com", new DateTime(1995, 5, 10));
}
