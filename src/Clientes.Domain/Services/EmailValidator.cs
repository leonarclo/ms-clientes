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

        // TryCreate também aceita o formato "João <joao@email.com>".
        // Num cadastro queremos apenas o endereço puro.
        if (endereco.Address != email)
            return false;

        // Exige domínio com ponto: rejeita "joao@localhost", que é
        // sintaticamente válido mas não serve para um cliente real.
        var dominio = endereco.Host;

        return dominio.Contains('.')
            && !dominio.StartsWith('.')
            && !dominio.EndsWith('.');
    }
}
