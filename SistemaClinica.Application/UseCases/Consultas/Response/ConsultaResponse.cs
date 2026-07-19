using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Consultas.Response;

public class ConsultaResponse
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public string PacienteNome { get; set; } = string.Empty;
    public int MedicoId { get; set; }
    public string MedicoNome { get; set; } = string.Empty;
    public int EspecialidadeId { get; set; }
    public string EspecialidadeDescricao { get; set; } = string.Empty;
    public DateTime DataConsulta { get; set; }
    public string Observacao { get; set; } = string.Empty;
    public SituacaoConsulta SituacaoConsulta { get; set; }
}
