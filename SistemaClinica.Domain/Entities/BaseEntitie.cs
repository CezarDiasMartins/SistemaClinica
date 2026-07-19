namespace SistemaClinica.Domain.Entities;

public abstract class BaseEntitie
{
    public int Id { get; set; }
    public int UsuarioIdInclusao { get; set; }
    public DateTime DataHoraInclusao { get; set; }
    public int? UsuarioIdAlteracao { get; set; }
    public DateTime? DataHoraAlteracao { get; set; }
}
