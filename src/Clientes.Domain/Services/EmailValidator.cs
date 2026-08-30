using System.Net.Mail;

namespace Clientes.Domain.Services;

public static class EmailValidator
{
    public static bool IsValid(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        email = email.Trim();

        if (!MailAddress.TryCreate(email, out var endereco))
            return false;

        // TryCreate aceita "João <joao@email.com>"; queremos só o endereço.
        if (endereco.Address != email)
            return false;

        // Rejeita "joao@localhost": válido pela RFC, inútil como cliente.
        var dominio = endereco.Host;

        return dominio.Contains('.')
            && !dominio.StartsWith('.')
            && !dominio.EndsWith('.');
    }
}
