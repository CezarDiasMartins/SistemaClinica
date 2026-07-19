using Mapster;
using SistemaClinica.Application.UseCases.Consultas.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Consultas.Mapper;

public class ConsultaMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Consulta, ConsultaResponse>()
            .Map(destino => destino.PacienteNome, origem => origem.Paciente == null ? string.Empty : origem.Paciente.Nome)
            .Map(destino => destino.MedicoNome, origem => origem.Medico == null ? string.Empty : origem.Medico.Nome)
            .Map(destino => destino.EspecialidadeDescricao, origem => origem.Especialidade == null ? string.Empty : origem.Especialidade.Descricao);
    }
}
