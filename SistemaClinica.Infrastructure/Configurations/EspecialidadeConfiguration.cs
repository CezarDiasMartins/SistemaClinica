using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Configurations;

public class EspecialidadeConfiguration : BaseEntitieConfiguration<Especialidade>
{
    public override void Configure(EntityTypeBuilder<Especialidade> builder)
    {
        base.Configure(builder);

        builder.ToTable("Especialidades");
        builder.Property(x => x.Descricao).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Descricao).IsUnique();
    }
}
