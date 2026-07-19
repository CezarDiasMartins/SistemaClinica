namespace SistemaClinica.Application.Interfaces;

public interface IUsuarioLogadoService
{
    int? UsuarioId { get; }
    string? Nome { get; }
    string? Email { get; }
    string? Role { get; }
}
