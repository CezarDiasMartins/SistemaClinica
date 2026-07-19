using Mapster;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.UseCases.Recepcionistas.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Mapper;

public class RecepcionistaMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Recepcionista, RecepcionistaResponse>()
            .Map(destino => destino.Situacao, origem => SituacaoGeralHelper.ToCodigo(origem.Situacao));
    }
}
