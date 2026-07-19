using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Response;

public class RecepcionistaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RoleUsuario Role { get; set; }
    public string Situacao { get; set; } = string.Empty;
}
