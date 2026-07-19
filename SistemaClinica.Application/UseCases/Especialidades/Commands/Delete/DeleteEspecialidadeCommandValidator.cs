using FluentValidation;

namespace SistemaClinica.Application.UseCases.Especialidades.Commands.Delete;

public class DeleteEspecialidadeCommandValidator : AbstractValidator<DeleteEspecialidadeCommand>
{
    public DeleteEspecialidadeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador da especialidade.");
    }
}
