using FluentValidation;

namespace SistemaClinica.Application.UseCases.Consultas.Commands.Delete;

public class DeleteConsultaCommandValidator : AbstractValidator<DeleteConsultaCommand>
{
    public DeleteConsultaCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador da consulta.");
    }
}
