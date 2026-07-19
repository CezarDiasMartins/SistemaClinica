using System.Text;

namespace SistemaClinica.Libs.Extensions;

public static class StringExtensions
{
    public static string SoNumeros(this string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return string.Empty;

        var resultado = new StringBuilder(valor.Length);

        foreach (var caractere in valor)
            if (char.IsDigit(caractere))
                resultado.Append(caractere);

        return resultado.ToString();
    }

    public static string NormalizarEmail(this string? email)
    {
        return email?.Trim().ToLowerInvariant() ?? string.Empty;
    }
}
