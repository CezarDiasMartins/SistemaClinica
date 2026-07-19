using FluentValidation;

namespace SistemaClinica.Application.UseCases.Especialidades.Commands.Update;

public class UpdateEspecialidadeCommandValidator : EspecialidadeCommandBaseValidator<UpdateEspecialidadeCommand>
{
    public UpdateEspecialidadeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador da especialidade.");
    }
}
