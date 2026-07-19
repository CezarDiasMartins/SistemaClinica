using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Configurations;

public class ConsultaConfiguration : BaseEntitieConfiguration<Consulta>
{
    public override void Configure(EntityTypeBuilder<Consulta> builder)
    {
        base.Configure(builder);

        builder.ToTable("Consultas");
        builder.Property(x => x.DataConsulta)
            .HasColumnType("timestamp without time zone")
            .IsRequired();
        builder.Property(x => x.Observacao).HasMaxLength(500);
        builder.Property(x => x.SituacaoConsulta)
            .HasConversion<int>()
            .HasComment("Situação da consulta: 1 - Agendada, 2 - Confirmada, 3 - Cancelada, 4 - Finalizada.")
            .IsRequired();
        builder.HasIndex(x => x.MedicoId);
        builder.HasIndex(x => x.EspecialidadeId);
        builder.HasIndex(x => x.DataConsulta);

        builder.HasOne(x => x.Paciente)
            .WithMany(x => x.Consultas)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Medico)
            .WithMany(x => x.Consultas)
            .HasForeignKey(x => x.MedicoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Especialidade)
            .WithMany(x => x.Consultas)
            .HasForeignKey(x => x.EspecialidadeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
