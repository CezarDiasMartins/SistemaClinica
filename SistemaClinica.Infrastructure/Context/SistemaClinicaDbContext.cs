using Microsoft.EntityFrameworkCore;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Context;

public class SistemaClinicaDbContext(
    DbContextOptions<SistemaClinicaDbContext> options,
    IUsuarioLogadoService? usuarioLogadoService = null)
    : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Administrador> Administradores => Set<Administrador>();
    public DbSet<Recepcionista> Recepcionistas => Set<Recepcionista>();
    public DbSet<Medico> Medicos => Set<Medico>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<Especialidade> Especialidades => Set<Especialidade>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SistemaClinicaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AplicarAuditoria();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AplicarAuditoria()
    {
        var agora = DateTime.UtcNow;
        var usuarioId = usuarioLogadoService?.UsuarioId;

        foreach (var entry in ChangeTracker.Entries<BaseEntitie>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DataHoraInclusao = agora;
                entry.Entity.UsuarioIdInclusao = usuarioId ?? 0;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.DataHoraAlteracao = agora;
                entry.Entity.UsuarioIdAlteracao = usuarioId;
            }
        }
    }
}
