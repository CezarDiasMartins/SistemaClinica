using Mapster;
using SistemaClinica.Application.UseCases.Especialidades.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Especialidades.Mapper;

public class EspecialidadeMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Especialidade, EspecialidadeResponse>();
    }
}
