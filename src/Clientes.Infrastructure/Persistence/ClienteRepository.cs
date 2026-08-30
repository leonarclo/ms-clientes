using Clientes.Application.Abstractions;
using Clientes.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Clientes.Infrastructure.Persistence;

public sealed class ClienteRepository(string connectionString) : IClienteRepository
{
    private const string SqlObterPorId = """
        SELECT Id, Nome, Cpf, Email, DataNascimento, DataCadastro
        FROM dbo.Clientes
        WHERE Id = @Id;
        """;

    private const string SqlExisteCpf = """
        SELECT CASE WHEN EXISTS (
            SELECT 1 FROM dbo.Clientes WHERE Cpf = @Cpf
        ) THEN 1 ELSE 0 END;
        """;

    private const string SqlInserir = """
        INSERT INTO dbo.Clientes (Id, Nome, Cpf, Email, DataNascimento, DataCadastro)
        VALUES (@Id, @Nome, @Cpf, @Email, @DataNascimento, @DataCadastro);
        """;

    public async Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var conexao = new SqlConnection(connectionString);

        return await conexao.QuerySingleOrDefaultAsync<Cliente>(new CommandDefinition(
            SqlObterPorId,
            new { Id = id },
            cancellationToken: cancellationToken));
    }

    public async Task<bool> ExisteCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        await using var conexao = new SqlConnection(connectionString);

        return await conexao.ExecuteScalarAsync<bool>(new CommandDefinition(
            SqlExisteCpf,
            new { Cpf = cpf },
            cancellationToken: cancellationToken));
    }

    public async Task InserirAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await using var conexao = new SqlConnection(connectionString);

        await conexao.ExecuteAsync(new CommandDefinition(
            SqlInserir,
            new
            {
                cliente.Id,
                cliente.Nome,
                cliente.Cpf,
                cliente.Email,
                cliente.DataNascimento,
                cliente.DataCadastro
            },
            cancellationToken: cancellationToken));
    }
}
