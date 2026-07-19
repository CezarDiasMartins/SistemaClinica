using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Medicos.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Medicos.Queries.Get;

public class GetMedicoQuery : IRequest<GenericDataResponse<MedicoResponse>>
{
    public int Id { get; set; }
}

public class GetMedicoQueryHandler(IRepository<Medico> medicoRepository, IMapper _mapper)
    : IRequestHandler<GetMedicoQuery, GenericDataResponse<MedicoResponse>>
{
    public async Task<GenericDataResponse<MedicoResponse>> Handle(GetMedicoQuery request, CancellationToken cancellationToken)
    {
        var medico = await medicoRepository.GetByIdAsync(request.Id, cancellationToken);
        return medico is null
            ? new GenericDataResponse<MedicoResponse> { Errors = ["Médico não encontrado."] }
            : new GenericDataResponse<MedicoResponse> { Data = _mapper.Map<MedicoResponse>(medico) };
    }
}
