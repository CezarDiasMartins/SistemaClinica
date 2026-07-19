using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Recepcionistas.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Queries.List;

public class ListRecepcionistaQuery : IRequest<GenericDataResponse<List<RecepcionistaResponse>>>;

public class ListRecepcionistaQueryHandler(IRepository<Recepcionista> recepcionistaRepository, IMapper _mapper)
    : IRequestHandler<ListRecepcionistaQuery, GenericDataResponse<List<RecepcionistaResponse>>>
{
    public async Task<GenericDataResponse<List<RecepcionistaResponse>>> Handle(ListRecepcionistaQuery request, CancellationToken cancellationToken)
    {
        var recepcionistas = await recepcionistaRepository.GetAllAsync(cancellationToken);
        return new GenericDataResponse<List<RecepcionistaResponse>> { Data = _mapper.Map<List<RecepcionistaResponse>>(recepcionistas) };
    }
}
