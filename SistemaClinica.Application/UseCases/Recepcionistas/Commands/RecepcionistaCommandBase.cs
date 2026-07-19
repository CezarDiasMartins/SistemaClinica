using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Commands;

public abstract class RecepcionistaCommandBase
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
