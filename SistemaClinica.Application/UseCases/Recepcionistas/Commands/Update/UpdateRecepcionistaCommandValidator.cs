using FluentValidation;
using SistemaClinica.Application.Helpers;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Commands.Update;

public class UpdateRecepcionistaCommandValidator : RecepcionistaCommandBaseValidator<UpdateRecepcionistaCommand>
{
    public UpdateRecepcionistaCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador do recepcionista.");
        RuleFor(x => x.Situacao)
            .NotEmpty().WithMessage("Informe a situação do recepcionista.")
            .Must(situacao => SituacaoGeralHelper.TryParse(situacao, out _)).WithMessage("Informe uma situação válida para o recepcionista.");

        When(x => !string.IsNullOrWhiteSpace(x.Senha), () =>
        {
            RuleFor(x => x.Senha).MinimumLength(8).WithMessage("A senha do recepcionista deve ter pelo menos 8 caracteres.");
        });
    }
}
