using Mapster;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.UseCases.Medicos.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Medicos.Mapper;

public class MedicoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Medico, MedicoResponse>()
            .Map(destino => destino.Situacao, origem => SituacaoGeralHelper.ToCodigo(origem.Situacao))
            .Map(destino => destino.EspecialidadeDescricao, origem => origem.Especialidade == null ? string.Empty : origem.Especialidade.Descricao);
    }
}
