using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Consultas.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Consultas.Queries.List;

public class ListConsultaQuery : IRequest<GenericDataResponse<List<ConsultaResponse>>>;

public class ListConsultaQueryHandler(IRepository<Consulta> consultaRepository, IMapper _mapper)
    : IRequestHandler<ListConsultaQuery, GenericDataResponse<List<ConsultaResponse>>>
{
    public async Task<GenericDataResponse<List<ConsultaResponse>>> Handle(ListConsultaQuery request, CancellationToken cancellationToken)
    {
        var consultas = await consultaRepository.SearchAsync(null, query => query.OrderBy(x => x.DataConsulta), 0, 500, cancellationToken, c => c.Paciente!, c => c.Medico!, c => c.Especialidade!);
        return new GenericDataResponse<List<ConsultaResponse>> { Data = _mapper.Map<List<ConsultaResponse>>(consultas) };
    }
}
