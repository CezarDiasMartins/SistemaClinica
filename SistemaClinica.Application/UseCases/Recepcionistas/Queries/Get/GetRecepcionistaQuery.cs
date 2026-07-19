using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Recepcionistas.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Queries.Get;

public class GetRecepcionistaQuery : IRequest<GenericDataResponse<RecepcionistaResponse>>
{
    public int Id { get; set; }
}

public class GetRecepcionistaQueryHandler(IRepository<Recepcionista> recepcionistaRepository, IMapper _mapper)
    : IRequestHandler<GetRecepcionistaQuery, GenericDataResponse<RecepcionistaResponse>>
{
    public async Task<GenericDataResponse<RecepcionistaResponse>> Handle(GetRecepcionistaQuery request, CancellationToken cancellationToken)
    {
        var recepcionista = await recepcionistaRepository.GetByIdAsync(request.Id, cancellationToken);

        return recepcionista is null
            ? new GenericDataResponse<RecepcionistaResponse> { Errors = ["Recepcionista não encontrado."] }
            : new GenericDataResponse<RecepcionistaResponse> { Data = _mapper.Map<RecepcionistaResponse>(recepcionista) };
    }
}
