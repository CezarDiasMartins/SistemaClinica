using SistemaClinica.Domain.Enums;
using SistemaClinica.Application.Helpers;

namespace SistemaClinica.Application.UseCases.Consultas.Commands;

public abstract class ConsultaCommandBase
{
    private DateTime _dataConsulta;

    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public int EspecialidadeId { get; set; }
    public DateTime DataConsulta
    {
        get => _dataConsulta;
        set => _dataConsulta = DataHoraHelper.ParaLocalSemFuso(value);
    }
    public string Observacao { get; set; } = string.Empty;
    public SituacaoConsulta SituacaoConsulta { get; set; } = SituacaoConsulta.Agendada;
}
