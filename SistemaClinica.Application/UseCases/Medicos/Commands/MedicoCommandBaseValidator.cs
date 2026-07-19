using FluentValidation;

namespace SistemaClinica.Application.UseCases.Medicos.Commands;

public class MedicoCommandBaseValidator<T> : AbstractValidator<T> where T : MedicoCommandBase
{
    public MedicoCommandBaseValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().WithMessage("Informe o nome do médico.").MaximumLength(150);
        RuleFor(x => x.CRM).NotEmpty().WithMessage("Informe o CRM do médico.").MaximumLength(20);
        RuleFor(x => x.EspecialidadeId).GreaterThan(0).WithMessage("Informe a especialidade do médico.");
        RuleFor(x => x.Telefone).NotEmpty().WithMessage("Informe o telefone do médico.").MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Informe o e-mail do médico.").EmailAddress().WithMessage("Informe um e-mail de médico válido.").MaximumLength(150);
    }
}
