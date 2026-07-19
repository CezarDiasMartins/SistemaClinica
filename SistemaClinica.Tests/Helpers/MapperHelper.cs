using Mapster;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Services;

namespace SistemaClinica.Tests.Helpers;

public static class MapperHelper
{
    public static IMapper Mapper { get; } = CriarMapper();

    private static IMapper CriarMapper()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(SistemaClinica.Application.Extensions.DependencyInjection).Assembly);

        return new Mapper(config);
    }
}
