using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.Request;

public class ListPagedRequest
{
    public int Pagina { get; set; } = 1;
    public int QuantidadeRegistros { get; set; } = 10;
    public string? OrderBy { get; set; }
    public DirecaoOrdenacao DirectionOrder { get; set; } = DirecaoOrdenacao.Asc;
}
