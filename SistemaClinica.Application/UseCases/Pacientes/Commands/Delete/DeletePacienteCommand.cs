using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Pacientes.Commands.Delete;

public class DeletePacienteCommand : IRequest<GenericNoDataResponse>
{
    public int Id { get; set; }
}

public class DeletePacienteCommandHandler(IRepository<Paciente> pacienteRepository)
    : IRequestHandler<DeletePacienteCommand, GenericNoDataResponse>
{
    public async Task<GenericNoDataResponse> Handle(DeletePacienteCommand request, CancellationToken cancellationToken)
    {
        var paciente = await pacienteRepository.GetByIdAsync(request.Id, cancellationToken);

        if (paciente is null)
            return new GenericNoDataResponse { Errors = ["Paciente não encontrado."] };

        pacienteRepository.Delete(paciente);
        await pacienteRepository.SalveAsync(cancellationToken);

        return new GenericNoDataResponse();
    }
}
