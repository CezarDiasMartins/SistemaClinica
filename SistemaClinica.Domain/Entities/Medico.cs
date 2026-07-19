using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Domain.Entities;

public class Medico : Usuario
{
    public Medico()
    {
        Role = RoleUsuario.Medico;
    }

    public string CRM { get; set; } = string.Empty;
    public int EspecialidadeId { get; set; }
    public string Telefone { get; set; } = string.Empty;

    public virtual Especialidade? Especialidade { get; set; }
    public virtual ICollection<Consulta> Consultas { get; set; } = [];
}
