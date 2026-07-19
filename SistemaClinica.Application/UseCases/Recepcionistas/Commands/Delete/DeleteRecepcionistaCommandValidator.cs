using FluentValidation;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Commands.Delete;

public class DeleteRecepcionistaCommandValidator : AbstractValidator<DeleteRecepcionistaCommand>
{
    public DeleteRecepcionistaCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador do recepcionista.");
    }
}
