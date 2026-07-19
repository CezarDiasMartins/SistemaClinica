using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Middlewares;
using SistemaClinica.Application.Services;
using System.Reflection;

namespace SistemaClinica.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var mapperConfig = TypeAdapterConfig.GlobalSettings;
        mapperConfig.Scan(assembly);

        services.AddSingleton(mapperConfig);
        services.AddScoped<IMapper, Mapper>();
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
