using System.Linq.Expressions;
using MediatR;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Request;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Medicos.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Medicos.Queries.Search;

public class SearchMedicoQuery : ListPagedRequest, IRequest<ListPagedResponse<MedicoResponse>>
{
    public string? Nome { get; set; }
    public string? CRM { get; set; }
    public int? EspecialidadeId { get; set; }
    public string? Situacao { get; set; }
}

public class SearchMedicoQueryHandler(IRepository<Medico> medicoRepository, IMapper _mapper)
    : IRequestHandler<SearchMedicoQuery, ListPagedResponse<MedicoResponse>>
{
    public async Task<ListPagedResponse<MedicoResponse>> Handle(SearchMedicoQuery request, CancellationToken cancellationToken)
    {
        var possuiSituacaoValida = SituacaoGeralHelper.TryParse(request.Situacao, out var situacao);

        Expression<Func<Medico, bool>> filtro = medico =>
            (string.IsNullOrWhiteSpace(request.Nome) || medico.Nome.ToLower().Contains(request.Nome.ToLower())) &&
            (string.IsNullOrWhiteSpace(request.CRM) || medico.CRM.Contains(request.CRM.ToUpper())) &&
            (!request.EspecialidadeId.HasValue || medico.EspecialidadeId == request.EspecialidadeId.Value) &&
            (string.IsNullOrWhiteSpace(request.Situacao) || (possuiSituacaoValida && medico.Situacao == situacao));

        var total = await medicoRepository.CountAsync(filtro, cancellationToken);
        var medicos = await medicoRepository.SearchAsync(
            filtro,
            Ordenar(request),
            PaginacaoHelper.CalcularSkip(request),
            PaginacaoHelper.NormalizarQuantidade(request.QuantidadeRegistros),
            cancellationToken,
            medico => medico.Especialidade!);

        return PaginacaoHelper.CriarResposta(_mapper.Map<List<MedicoResponse>>(medicos), request, total);
    }

    private static Func<IQueryable<Medico>, IOrderedQueryable<Medico>> Ordenar(SearchMedicoQuery request)
    {
        var desc = request.DirectionOrder == DirecaoOrdenacao.Desc;
        return request.OrderBy?.ToLowerInvariant() switch
        {
            "crm" => query => desc ? query.OrderByDescending(x => x.CRM) : query.OrderBy(x => x.CRM),
            "especialidade" => query => desc ? query.OrderByDescending(x => x.Especialidade!.Descricao) : query.OrderBy(x => x.Especialidade!.Descricao),
            _ => query => desc ? query.OrderByDescending(x => x.Nome) : query.OrderBy(x => x.Nome)
        };
    }
}
