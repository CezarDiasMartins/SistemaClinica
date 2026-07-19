using FluentValidation;

namespace SistemaClinica.Application.UseCases.Pacientes.Commands.Delete;

public class DeletePacienteCommandValidator : AbstractValidator<DeletePacienteCommand>
{
    public DeletePacienteCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Informe o identificador do paciente.");
    }
}
