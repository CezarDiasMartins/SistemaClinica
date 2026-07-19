using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Commands.Delete;

public class DeleteRecepcionistaCommand : IRequest<GenericNoDataResponse>
{
    public int Id { get; set; }
}

public class DeleteRecepcionistaCommandHandler(IRepository<Recepcionista> recepcionistaRepository)
    : IRequestHandler<DeleteRecepcionistaCommand, GenericNoDataResponse>
{
    public async Task<GenericNoDataResponse> Handle(DeleteRecepcionistaCommand request, CancellationToken cancellationToken)
    {
        var recepcionista = await recepcionistaRepository.GetByIdAsync(request.Id, cancellationToken);

        if (recepcionista is null)
            return new GenericNoDataResponse { Errors = ["Recepcionista não encontrado."] };

        recepcionistaRepository.Delete(recepcionista);
        await recepcionistaRepository.SalveAsync(cancellationToken);

        return new GenericNoDataResponse();
    }
}
