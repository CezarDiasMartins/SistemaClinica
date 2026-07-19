namespace SistemaClinica.Application.Response;

public class GenericNoDataResponse
{
    public bool Success => !Errors.Any();
    public List<string> Errors { get; set; } = [];
}
