using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Domain.Entities;

public class Administrador : Usuario
{
    public Administrador()
    {
        Role = RoleUsuario.Administrador;
    }
}
