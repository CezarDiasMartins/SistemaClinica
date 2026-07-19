using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Pacientes.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Pacientes.Queries.List;

public class ListPacienteQuery : IRequest<GenericDataResponse<List<PacienteResponse>>>;

public class ListPacienteQueryHandler(IRepository<Paciente> pacienteRepository, IMapper _mapper)
    : IRequestHandler<ListPacienteQuery, GenericDataResponse<List<PacienteResponse>>>
{
    public async Task<GenericDataResponse<List<PacienteResponse>>> Handle(ListPacienteQuery request, CancellationToken cancellationToken)
    {
        var pacientes = await pacienteRepository.GetAllAsync(cancellationToken);
        return new GenericDataResponse<List<PacienteResponse>> { Data = _mapper.Map<List<PacienteResponse>>(pacientes) };
    }
}
