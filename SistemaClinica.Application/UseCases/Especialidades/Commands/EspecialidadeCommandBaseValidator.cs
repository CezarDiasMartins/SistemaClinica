using FluentValidation;

namespace SistemaClinica.Application.UseCases.Especialidades.Commands;

public class EspecialidadeCommandBaseValidator<T> : AbstractValidator<T> where T : EspecialidadeCommandBase
{
    public EspecialidadeCommandBaseValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Informe a descrição da especialidade.")
            .MaximumLength(100).WithMessage("A descrição da especialidade deve ter no máximo 100 caracteres.");
    }
}
