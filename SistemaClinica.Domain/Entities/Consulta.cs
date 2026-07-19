using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Domain.Entities;

public class Consulta : BaseEntitie
{
    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public int EspecialidadeId { get; set; }
    public DateTime DataConsulta { get; set; }
    public string Observacao { get; set; } = string.Empty;
    public SituacaoConsulta SituacaoConsulta { get; set; } = SituacaoConsulta.Agendada;
    
    public virtual Paciente? Paciente { get; set; }
    public virtual Medico? Medico { get; set; }
    public virtual Especialidade? Especialidade { get; set; }
}
