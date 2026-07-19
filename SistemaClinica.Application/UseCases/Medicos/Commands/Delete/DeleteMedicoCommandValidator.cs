using FluentValidation;

namespace SistemaClinica.Application.UseCases.Medicos.Commands.Delete;

public class DeleteMedicoCommandValidator : AbstractValidator<DeleteMedicoCommand>
{
    public DeleteMedicoCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador do médico.");
    }
}
