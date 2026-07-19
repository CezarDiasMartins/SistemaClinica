using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Pacientes.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Pacientes.Queries.Get;

public class GetPacienteQuery : IRequest<GenericDataResponse<PacienteResponse>>
{
    public int Id { get; set; }
}

public class GetPacienteQueryHandler(IRepository<Paciente> pacienteRepository, IMapper _mapper)
    : IRequestHandler<GetPacienteQuery, GenericDataResponse<PacienteResponse>>
{
    public async Task<GenericDataResponse<PacienteResponse>> Handle(GetPacienteQuery request, CancellationToken cancellationToken)
    {
        var paciente = await pacienteRepository.GetByIdAsync(request.Id, cancellationToken);
        return paciente is null
            ? new GenericDataResponse<PacienteResponse> { Errors = ["Paciente não encontrado."] }
            : new GenericDataResponse<PacienteResponse> { Data = _mapper.Map<PacienteResponse>(paciente) };
    }
}
