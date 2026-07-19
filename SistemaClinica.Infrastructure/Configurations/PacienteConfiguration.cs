using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Configurations;

public class PacienteConfiguration : BaseEntitieConfiguration<Paciente>
{
    public override void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("Pacientes");
        builder.Property(x => x.CPF).HasMaxLength(11).IsRequired();
        builder.Property(x => x.DataNascimento).IsRequired();
        builder.Property(x => x.Telefone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Sexo).HasConversion<int>().IsRequired();
        builder.HasIndex(x => x.CPF).IsUnique();
    }
}
