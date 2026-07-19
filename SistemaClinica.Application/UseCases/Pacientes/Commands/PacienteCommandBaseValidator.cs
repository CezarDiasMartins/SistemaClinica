using FluentValidation;
using SistemaClinica.Libs.Extensions;

namespace SistemaClinica.Application.UseCases.Pacientes.Commands;

public class PacienteCommandBaseValidator<T> : AbstractValidator<T> where T : PacienteCommandBase
{
    public PacienteCommandBaseValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().WithMessage("Informe o nome do paciente.").MaximumLength(150);
        RuleFor(x => x.CPF).Must(cpf => cpf.SoNumeros().Length == 11).WithMessage("Informe um CPF válido para o paciente.");
        RuleFor(x => x.DataNascimento).LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("A data de nascimento do paciente deve ser anterior a hoje.");
        RuleFor(x => x.Telefone).NotEmpty().WithMessage("Informe o telefone do paciente.").MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Informe o e-mail do paciente.").EmailAddress().WithMessage("Informe um e-mail de paciente válido.").MaximumLength(150);
        RuleFor(x => x.Sexo).IsInEnum().WithMessage("Informe um sexo válido para o paciente.");
    }
}
