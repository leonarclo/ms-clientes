namespace Clientes.Domain.Services;

public static class CpfValidator
{
    public static bool IsValid(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = Normalizar(cpf);

        if (cpf.Length != 11)
            return false;

        // Passam no cálculo, mas não existem na Receita Federal.
        if (cpf.Distinct().Count() == 1)
            return false;

        return DigitoVerificador(cpf, 9) == (cpf[9] - '0')
            && DigitoVerificador(cpf, 10) == (cpf[10] - '0');
    }

    public static string Normalizar(string cpf) =>
        new string(cpf.Where(char.IsDigit).ToArray());

    private static int DigitoVerificador(string cpf, int quantidadeDigitos)
    {
        var soma = 0;
        var peso = quantidadeDigitos + 1;

        for (var i = 0; i < quantidadeDigitos; i++)
            soma += (cpf[i] - '0') * (peso - i);

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
