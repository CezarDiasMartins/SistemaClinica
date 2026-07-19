using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Medicos.Commands.Delete;

public class DeleteMedicoCommand : IRequest<GenericNoDataResponse>
{
    public int Id { get; set; }
}

public class DeleteMedicoCommandHandler(IRepository<Medico> medicoRepository)
    : IRequestHandler<DeleteMedicoCommand, GenericNoDataResponse>
{
    public async Task<GenericNoDataResponse> Handle(DeleteMedicoCommand request, CancellationToken cancellationToken)
    {
        var medico = await medicoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (medico is null)
            return new GenericNoDataResponse { Errors = ["Médico não encontrado."] };

        medicoRepository.Delete(medico);
        await medicoRepository.SalveAsync(cancellationToken);

        return new GenericNoDataResponse();
    }
}
