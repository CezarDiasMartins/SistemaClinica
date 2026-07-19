using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SistemaClinica.Infrastructure.Context;

public class SistemaClinicaDbContextFactory : IDesignTimeDbContextFactory<SistemaClinicaDbContext>
{
    public SistemaClinicaDbContext CreateDbContext(string[] args)
    {
        var apiPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "SistemaClinica.API"));
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

        var options = new DbContextOptionsBuilder<SistemaClinicaDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new SistemaClinicaDbContext(options);
    }
}
