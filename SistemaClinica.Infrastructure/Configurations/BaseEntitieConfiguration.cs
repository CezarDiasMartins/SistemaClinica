using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Configurations;

public abstract class BaseEntitieConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntitie
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.DataHoraInclusao).IsRequired();
        builder.Property(x => x.UsuarioIdInclusao).IsRequired();
        builder.Property(x => x.DataHoraAlteracao);
        builder.Property(x => x.UsuarioIdAlteracao);
    }
}
