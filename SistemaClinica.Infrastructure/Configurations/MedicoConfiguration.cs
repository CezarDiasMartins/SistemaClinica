using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Configurations;

public class MedicoConfiguration : BaseEntitieConfiguration<Medico>
{
    public override void Configure(EntityTypeBuilder<Medico> builder)
    {
        builder.ToTable("Medicos");
        builder.Property(x => x.CRM).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Telefone).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.CRM).IsUnique();

        builder.HasOne(x => x.Especialidade)
            .WithMany(x => x.Medicos)
            .HasForeignKey(x => x.EspecialidadeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
