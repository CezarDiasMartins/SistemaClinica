using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Domain.Entities;

public abstract class Usuario : BaseEntitie
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public RoleUsuario Role { get; set; }
    public SituacaoGeral Situacao { get; set; } = SituacaoGeral.Ativo;
}
