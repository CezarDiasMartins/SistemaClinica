using FluentValidation;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Commands.Create;

public class CreateRecepcionistaCommandValidator : RecepcionistaCommandBaseValidator<CreateRecepcionistaCommand>
{
    public CreateRecepcionistaCommandValidator()
    {
        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Informe a senha do recepcionista.")
            .MinimumLength(8).WithMessage("A senha do recepcionista deve ter pelo menos 8 caracteres.");
    }
}
