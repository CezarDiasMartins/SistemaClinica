using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Configurations;

public class RecepcionistaConfiguration : IEntityTypeConfiguration<Recepcionista>
{
    public void Configure(EntityTypeBuilder<Recepcionista> builder)
    {
        builder.ToTable("Recepcionistas");
    }
}
