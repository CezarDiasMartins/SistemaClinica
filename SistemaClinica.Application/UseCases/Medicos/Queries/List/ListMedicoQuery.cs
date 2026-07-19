using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Medicos.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Medicos.Queries.List;

public class ListMedicoQuery : IRequest<GenericDataResponse<List<MedicoResponse>>>;

public class ListMedicoQueryHandler(IRepository<Medico> medicoRepository, IMapper _mapper)
    : IRequestHandler<ListMedicoQuery, GenericDataResponse<List<MedicoResponse>>>
{
    public async Task<GenericDataResponse<List<MedicoResponse>>> Handle(ListMedicoQuery request, CancellationToken cancellationToken)
    {
        var medicos = await medicoRepository.GetAllAsync(cancellationToken);
        return new GenericDataResponse<List<MedicoResponse>> { Data = _mapper.Map<List<MedicoResponse>>(medicos) };
    }
}
