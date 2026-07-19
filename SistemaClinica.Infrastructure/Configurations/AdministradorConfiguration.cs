using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Infrastructure.Configurations;

public class AdministradorConfiguration : IEntityTypeConfiguration<Administrador>
{
    public void Configure(EntityTypeBuilder<Administrador> builder)
    {
        builder.ToTable("Administradores");

        builder.HasData(new Administrador
        {
            Id = 1,
            Nome = "Administrador",
            Email = "admin@clinica.com",
            Senha = "PBKDF2$100000$yQrfz4C53E0xBeII0RI26Q==$oIv3/q8P/66pPukotRclELVJ3itU7vVHHXi3ymbVp3o=", // Admin@123
            Role = RoleUsuario.Administrador,
            Situacao = SituacaoGeral.Ativo,
            UsuarioIdInclusao = 0,
            DataHoraInclusao = new DateTime(2026, 7, 11, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
