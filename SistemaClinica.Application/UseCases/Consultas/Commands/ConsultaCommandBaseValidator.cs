using FluentValidation;

namespace SistemaClinica.Application.UseCases.Consultas.Commands;

public class ConsultaCommandBaseValidator<T> : AbstractValidator<T> where T : ConsultaCommandBase
{
    public ConsultaCommandBaseValidator()
    {
        RuleFor(x => x.PacienteId).GreaterThan(0).WithMessage("Informe o paciente da consulta.");
        RuleFor(x => x.MedicoId).GreaterThan(0).WithMessage("Informe o médico da consulta.");
        RuleFor(x => x.EspecialidadeId).GreaterThan(0).WithMessage("Informe a especialidade da consulta.");
        RuleFor(x => x.DataConsulta).GreaterThan(DateTime.Now).WithMessage("A consulta não pode ser marcada em data passada.");
        RuleFor(x => x.Observacao).MaximumLength(500).WithMessage("A observação da consulta deve ter no máximo 500 caracteres.");
    }
}
