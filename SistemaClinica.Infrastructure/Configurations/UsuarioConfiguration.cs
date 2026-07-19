using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Configurations;

public class UsuarioConfiguration : BaseEntitieConfiguration<Usuario>
{
    public override void Configure(EntityTypeBuilder<Usuario> builder)
    {
        base.Configure(builder);

        builder.UseTptMappingStrategy();
        builder.ToTable("Usuarios");
        builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Senha).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Role).HasConversion<int>().IsRequired();
        builder.Property(x => x.Situacao)
            .HasConversion(
                situacao => (char)situacao,
                valor => (SistemaClinica.Domain.Enums.SituacaoGeral)valor)
            .HasMaxLength(1)
            .HasComment("Situação do usuário: A - Ativo, I - Inativo.")
            .IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
