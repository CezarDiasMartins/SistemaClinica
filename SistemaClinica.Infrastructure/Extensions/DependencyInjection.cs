using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Infrastructure.Context;
using SistemaClinica.Infrastructure.Repositories;
using SistemaClinica.Infrastructure.Security;

namespace SistemaClinica.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SistemaClinicaDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<JwtOptions>(options => configuration.GetSection("Jwt").Bind(options));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ISenhaService, SenhaService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
