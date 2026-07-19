using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Especialidades.Response;
using SistemaClinica.Domain.Entities;
using System.Text.Json.Serialization;

namespace SistemaClinica.Application.UseCases.Especialidades.Commands.Update;

public class UpdateEspecialidadeCommand : EspecialidadeCommandBase, IRequest<GenericDataResponse<EspecialidadeResponse>>
{
    [JsonIgnore]
    public int Id { get; set; }
}

public class UpdateEspecialidadeCommandHandler(IRepository<Especialidade> especialidadeRepository, IMapper _mapper)
    : IRequestHandler<UpdateEspecialidadeCommand, GenericDataResponse<EspecialidadeResponse>>
{
    public async Task<GenericDataResponse<EspecialidadeResponse>> Handle(UpdateEspecialidadeCommand request, CancellationToken cancellationToken)
    {
        var especialidade = await especialidadeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (especialidade is null)
            return new GenericDataResponse<EspecialidadeResponse> { Errors = ["Especialidade não encontrada."] };

        var descricao = request.Descricao.Trim();

        if (await especialidadeRepository.ExistsAsync(e => e.Descricao.ToLower() == descricao.ToLower() && e.Id != request.Id, cancellationToken))
            return new GenericDataResponse<EspecialidadeResponse> { Errors = ["Já existe outra especialidade cadastrada com esta descrição."] };

        especialidade.Descricao = descricao;

        especialidadeRepository.Update(especialidade);
        await especialidadeRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<EspecialidadeResponse> { Data = _mapper.Map<EspecialidadeResponse>(especialidade) };
    }
}
