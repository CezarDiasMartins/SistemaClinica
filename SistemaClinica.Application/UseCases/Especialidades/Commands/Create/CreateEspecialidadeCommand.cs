using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Especialidades.Response;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.UseCases.Especialidades.Commands.Create;

public class CreateEspecialidadeCommand : EspecialidadeCommandBase, IRequest<GenericDataResponse<EspecialidadeResponse>>;

public class CreateEspecialidadeCommandHandler(IRepository<Especialidade> especialidadeRepository, IMapper _mapper)
    : IRequestHandler<CreateEspecialidadeCommand, GenericDataResponse<EspecialidadeResponse>>
{
    public async Task<GenericDataResponse<EspecialidadeResponse>> Handle(CreateEspecialidadeCommand request, CancellationToken cancellationToken)
    {
        var descricao = request.Descricao.Trim();

        if (await especialidadeRepository.ExistsAsync(e => e.Descricao.ToLower() == descricao.ToLower(), cancellationToken))
            return new GenericDataResponse<EspecialidadeResponse> { Errors = ["Já existe uma especialidade cadastrada com esta descrição."] };

        var especialidade = new Especialidade
        {
            Descricao = descricao
        };

        await especialidadeRepository.InsertAsync(especialidade, cancellationToken);
        await especialidadeRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<EspecialidadeResponse> { Data = _mapper.Map<EspecialidadeResponse>(especialidade) };
    }
}
