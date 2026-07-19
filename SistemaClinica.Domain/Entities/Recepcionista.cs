using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Domain.Entities;

public class Recepcionista : Usuario
{
    public Recepcionista()
    {
        Role = RoleUsuario.Recepcionista;
    }
}
