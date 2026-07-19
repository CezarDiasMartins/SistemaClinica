using FluentValidation;
using SistemaClinica.Application.Helpers;

namespace SistemaClinica.Application.UseCases.Pacientes.Commands.Update;

public class UpdatePacienteCommandValidator : PacienteCommandBaseValidator<UpdatePacienteCommand>
{
    public UpdatePacienteCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador do paciente.");
        RuleFor(x => x.Situacao)
            .NotEmpty().WithMessage("Informe a situação do paciente.")
            .Must(situacao => SituacaoGeralHelper.TryParse(situacao, out _)).WithMessage("Informe uma situação válida para o paciente.");
    }
}
