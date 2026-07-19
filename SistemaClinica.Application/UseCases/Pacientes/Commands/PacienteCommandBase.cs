using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Pacientes.Commands;

public abstract class PacienteCommandBase
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Sexo Sexo { get; set; }
}
