using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Especialidades.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Especialidades.Queries.Get;

public class GetEspecialidadeQuery : IRequest<GenericDataResponse<EspecialidadeResponse>>
{
    public int Id { get; set; }
}

public class GetEspecialidadeQueryHandler(IRepository<Especialidade> especialidadeRepository, IMapper _mapper)
    : IRequestHandler<GetEspecialidadeQuery, GenericDataResponse<EspecialidadeResponse>>
{
    public async Task<GenericDataResponse<EspecialidadeResponse>> Handle(GetEspecialidadeQuery request, CancellationToken cancellationToken)
    {
        var especialidade = await especialidadeRepository.GetByIdAsync(request.Id, cancellationToken);
        return especialidade is null
            ? new GenericDataResponse<EspecialidadeResponse> { Errors = ["Especialidade não encontrada."] }
            : new GenericDataResponse<EspecialidadeResponse> { Data = _mapper.Map<EspecialidadeResponse>(especialidade) };
    }
}
