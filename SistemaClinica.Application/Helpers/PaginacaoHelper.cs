using SistemaClinica.Application.Request;
using SistemaClinica.Application.Response;

namespace SistemaClinica.Application.Helpers;

public static class PaginacaoHelper
{
    public static int CalcularSkip(ListPagedRequest request)
    {
        var pagina = request.Pagina <= 0 ? 1 : request.Pagina;
        var quantidade = NormalizarQuantidade(request.QuantidadeRegistros);

        return (pagina - 1) * quantidade;
    }

    public static int NormalizarQuantidade(int quantidade)
    {
        return quantidade <= 0 ? 10 : Math.Min(quantidade, 100);
    }

    public static ListPagedResponse<T> CriarResposta<T>(
        List<T> registros,
        ListPagedRequest request,
        int totalRegistros)
    {
        var quantidade = NormalizarQuantidade(request.QuantidadeRegistros);
        var pagina = request.Pagina <= 0 ? 1 : request.Pagina;

        return new ListPagedResponse<T>
        {
            Registros = registros,
            Pagina = pagina,
            QuantidadeRegistros = quantidade,
            TotalRegistros = totalRegistros,
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)quantidade)
        };
    }
}
