using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Especialidades.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Especialidades.Queries.List;

public class ListEspecialidadeQuery : IRequest<GenericDataResponse<List<EspecialidadeResponse>>>;

public class ListEspecialidadeQueryHandler(IRepository<Especialidade> especialidadeRepository, IMapper _mapper)
    : IRequestHandler<ListEspecialidadeQuery, GenericDataResponse<List<EspecialidadeResponse>>>
{
    public async Task<GenericDataResponse<List<EspecialidadeResponse>>> Handle(ListEspecialidadeQuery request, CancellationToken cancellationToken)
    {
        var especialidades = await especialidadeRepository.GetAllAsync(cancellationToken);
        return new GenericDataResponse<List<EspecialidadeResponse>> { Data = _mapper.Map<List<EspecialidadeResponse>>(especialidades) };
    }
}
