using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Consultas.Commands.Delete;

public class DeleteConsultaCommand : IRequest<GenericNoDataResponse>
{
    public int Id { get; set; }
}

public class DeleteConsultaCommandHandler(IRepository<Consulta> consultaRepository)
    : IRequestHandler<DeleteConsultaCommand, GenericNoDataResponse>
{
    public async Task<GenericNoDataResponse> Handle(DeleteConsultaCommand request, CancellationToken cancellationToken)
    {
        var consulta = await consultaRepository.GetByIdAsync(request.Id, cancellationToken);

        if (consulta is null)
            return new GenericNoDataResponse { Errors = ["Consulta não encontrada."] };

        consultaRepository.Delete(consulta);
        await consultaRepository.SalveAsync(cancellationToken);

        return new GenericNoDataResponse();
    }
}
