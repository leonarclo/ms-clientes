using Clientes.Application.Exceptions;
using Clientes.Application.UseCases;
using Clientes.Domain.Entities;
using Clientes.UnitTests.Fakes;

namespace Clientes.UnitTests.UseCases;

public class ObterClienteUseCaseTests
{
    private readonly ClienteRepositoryFake _repositorio = new();

    [Fact]
    public async Task DeveRetornarClienteExistente()
    {
        // Arrange
        var id = Guid.NewGuid();
        await _repositorio.InserirAsync(new Cliente(
            id, "João da Silva", "12345678909", "joao@email.com",
            new DateTime(1995, 5, 10), new DateTime(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc)));

        var useCase = new ObterClienteUseCase(_repositorio);

        // Act
        var response = await useCase.ExecutarAsync(id);

        // Assert
        Assert.Equal(id, response.Id);
        Assert.Equal("João da Silva", response.Nome);
        Assert.Equal("12345678909", response.Cpf);
    }

    [Fact]
    public async Task DeveLancarQuandoClienteNaoExiste()
    {
        // Arrange
        var useCase = new ObterClienteUseCase(_repositorio);

        // Act & Assert
        await Assert.ThrowsAsync<ClienteNaoEncontradoException>(
            () => useCase.ExecutarAsync(Guid.NewGuid()));
    }
}
