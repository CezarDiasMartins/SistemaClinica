using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.Helpers;

public static class SituacaoGeralHelper
{
    public static bool TryParse(string? valor, out SituacaoGeral situacao)
    {
        switch (valor?.Trim().ToUpperInvariant())
        {
            case "A":
            case "ATIVO":
                situacao = SituacaoGeral.Ativo;
                return true;
            case "I":
            case "INATIVO":
                situacao = SituacaoGeral.Inativo;
                return true;
            default:
                situacao = default;
                return false;
        }
    }

    public static string ToCodigo(SituacaoGeral situacao)
    {
        return ((char)situacao).ToString();
    }
}
