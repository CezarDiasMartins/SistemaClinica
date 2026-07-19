namespace SistemaClinica.Application.Response;

public class ListPagedResponse<T>
{
    public bool Success => !Errors.Any();
    public List<T> Registros { get; set; } = [];
    public int Pagina { get; set; }
    public int QuantidadeRegistros { get; set; }
    public int TotalRegistros { get; set; }
    public int TotalPaginas { get; set; }
    public bool Previous => Pagina > 1;
    public bool Next => Pagina < TotalPaginas;
    public List<string> Errors { get; set; } = [];
}
