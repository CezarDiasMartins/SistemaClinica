using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Medicos.Commands;

public abstract class MedicoCommandBase
{
    public string Nome { get; set; } = string.Empty;
    public string CRM { get; set; } = string.Empty;
    public int EspecialidadeId { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
