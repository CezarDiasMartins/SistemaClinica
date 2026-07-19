using Mapster;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.UseCases.Pacientes.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Pacientes.Mapper;

public class PacienteMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Paciente, PacienteResponse>()
            .Map(destino => destino.Situacao, origem => SituacaoGeralHelper.ToCodigo(origem.Situacao));
    }
}
