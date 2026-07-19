using FluentValidation;

namespace SistemaClinica.Application.UseCases.Consultas.Commands.Update;

public class UpdateConsultaCommandValidator : ConsultaCommandBaseValidator<UpdateConsultaCommand>
{
    public UpdateConsultaCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador da consulta.");
    }
}
