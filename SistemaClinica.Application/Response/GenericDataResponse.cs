namespace SistemaClinica.Application.Response;

public class GenericDataResponse<T>
{
    public bool Success => !Errors.Any();
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];
}
