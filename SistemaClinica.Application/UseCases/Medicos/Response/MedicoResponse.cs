using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Medicos.Response;

public class MedicoResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CRM { get; set; } = string.Empty;
    public int EspecialidadeId { get; set; }
    public string EspecialidadeDescricao { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
}
