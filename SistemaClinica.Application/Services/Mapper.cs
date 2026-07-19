using Mapster;
using SistemaClinica.Application.Interfaces;

namespace SistemaClinica.Application.Services;

public class Mapper(TypeAdapterConfig config) : IMapper
{
    public TDestination Map<TDestination>(object source)
    {
        return source.Adapt<TDestination>(config);
    }
}

public static class MapperFactory
{
    private static readonly Lazy<IMapper> Mapper = new(CriarMapper);

    public static IMapper Default => Mapper.Value;

    private static IMapper CriarMapper()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MapperFactory).Assembly);

        return new Mapper(config);
    }
}
