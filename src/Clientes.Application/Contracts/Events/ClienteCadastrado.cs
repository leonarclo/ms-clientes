namespace Contracts.Events;

public sealed record ClienteCadastrado(
    Guid EventId,
    Guid ClienteId,
    string Nome,
    string Cpf,
    string Email,
    DateOnly DataNascimento,
    DateTimeOffset OccurredAt);
