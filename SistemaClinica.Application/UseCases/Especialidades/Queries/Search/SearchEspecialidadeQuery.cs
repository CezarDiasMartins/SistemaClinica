using System.Linq.Expressions;
using MediatR;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Request;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Especialidades.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Especialidades.Queries.Search;

public class SearchEspecialidadeQuery : ListPagedRequest, IRequest<ListPagedResponse<EspecialidadeResponse>>
{
    public string? Descricao { get; set; }
}

public class SearchEspecialidadeQueryHandler(IRepository<Especialidade> especialidadeRepository, IMapper _mapper)
    : IRequestHandler<SearchEspecialidadeQuery, ListPagedResponse<EspecialidadeResponse>>
{
    public async Task<ListPagedResponse<EspecialidadeResponse>> Handle(SearchEspecialidadeQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Especialidade, bool>> filtro = especialidade =>
            string.IsNullOrWhiteSpace(request.Descricao) || especialidade.Descricao.ToLower().Contains(request.Descricao.ToLower());

        var total = await especialidadeRepository.CountAsync(filtro, cancellationToken);
        var especialidades = await especialidadeRepository.SearchAsync(filtro, Ordenar(request), PaginacaoHelper.CalcularSkip(request), PaginacaoHelper.NormalizarQuantidade(request.QuantidadeRegistros), cancellationToken);

        return PaginacaoHelper.CriarResposta(_mapper.Map<List<EspecialidadeResponse>>(especialidades), request, total);
    }

    private static Func<IQueryable<Especialidade>, IOrderedQueryable<Especialidade>> Ordenar(SearchEspecialidadeQuery request)
    {
        var desc = request.DirectionOrder == DirecaoOrdenacao.Desc;
        return query => desc
            ? query.OrderByDescending(x => x.Descricao)
            : query.OrderBy(x => x.Descricao);
    }
}
