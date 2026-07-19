using FluentValidation;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Commands;

public class RecepcionistaCommandBaseValidator<T> : AbstractValidator<T> where T : RecepcionistaCommandBase
{
    public RecepcionistaCommandBaseValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Informe o nome do recepcionista.")
            .MaximumLength(150).WithMessage("O nome do recepcionista deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Informe o e-mail do recepcionista.")
            .EmailAddress().WithMessage("Informe um e-mail de recepcionista válido.")
            .MaximumLength(150).WithMessage("O e-mail do recepcionista deve ter no máximo 150 caracteres.");
    }
}
