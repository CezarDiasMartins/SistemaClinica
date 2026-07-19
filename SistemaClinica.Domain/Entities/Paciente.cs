using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Domain.Entities;

public class Paciente : Usuario
{
    public Paciente()
    {
        Role = RoleUsuario.Paciente;
    }

    public string CPF { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public Sexo Sexo { get; set; }

    public virtual ICollection<Consulta> Consultas { get; set; } = [];
}
