using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Especialidades.Commands.Delete;

public class DeleteEspecialidadeCommand : IRequest<GenericNoDataResponse>
{
    public int Id { get; set; }
}

public class DeleteEspecialidadeCommandHandler(IRepository<Especialidade> especialidadeRepository)
    : IRequestHandler<DeleteEspecialidadeCommand, GenericNoDataResponse>
{
    public async Task<GenericNoDataResponse> Handle(DeleteEspecialidadeCommand request, CancellationToken cancellationToken)
    {
        var especialidade = await especialidadeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (especialidade is null)
            return new GenericNoDataResponse { Errors = ["Especialidade não encontrada."] };

        especialidadeRepository.Delete(especialidade);
        await especialidadeRepository.SalveAsync(cancellationToken);

        return new GenericNoDataResponse();
    }
}
