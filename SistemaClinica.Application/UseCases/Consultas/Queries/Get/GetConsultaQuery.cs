using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Consultas.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Consultas.Queries.Get;

public class GetConsultaQuery : IRequest<GenericDataResponse<ConsultaResponse>>
{
    public int Id { get; set; }
}

public class GetConsultaQueryHandler(IRepository<Consulta> consultaRepository, IMapper _mapper)
    : IRequestHandler<GetConsultaQuery, GenericDataResponse<ConsultaResponse>>
{
    public async Task<GenericDataResponse<ConsultaResponse>> Handle(GetConsultaQuery request, CancellationToken cancellationToken)
    {
        var consulta = await consultaRepository.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken, c => c.Paciente!, c => c.Medico!, c => c.Especialidade!);
        return consulta is null
            ? new GenericDataResponse<ConsultaResponse> { Errors = ["Consulta não encontrada."] }
            : new GenericDataResponse<ConsultaResponse> { Data = _mapper.Map<ConsultaResponse>(consulta) };
    }
}
