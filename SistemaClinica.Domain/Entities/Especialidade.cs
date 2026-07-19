namespace SistemaClinica.Domain.Entities;

public class Especialidade : BaseEntitie
{
    public string Descricao { get; set; } = string.Empty;

    public virtual ICollection<Medico> Medicos { get; set; } = [];
    public virtual ICollection<Consulta> Consultas { get; set; } = [];
}
