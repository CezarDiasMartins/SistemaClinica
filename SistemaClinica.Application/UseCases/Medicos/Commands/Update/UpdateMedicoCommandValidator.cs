using FluentValidation;
using SistemaClinica.Application.Helpers;

namespace SistemaClinica.Application.UseCases.Medicos.Commands.Update;

public class UpdateMedicoCommandValidator : MedicoCommandBaseValidator<UpdateMedicoCommand>
{
    public UpdateMedicoCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador do médico.");
        RuleFor(x => x.Situacao)
            .NotEmpty().WithMessage("Informe a situação do médico.")
            .Must(situacao => SituacaoGeralHelper.TryParse(situacao, out _)).WithMessage("Informe uma situação válida para o médico.");
    }
}
