using System.Linq.Expressions;
using MediatR;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Request;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Recepcionistas.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;
using SistemaClinica.Libs.Extensions;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Queries.Search;

public class SearchRecepcionistaQuery : ListPagedRequest, IRequest<ListPagedResponse<RecepcionistaResponse>>
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Situacao { get; set; }
}

public class SearchRecepcionistaQueryHandler(IRepository<Recepcionista> recepcionistaRepository, IMapper _mapper)
    : IRequestHandler<SearchRecepcionistaQuery, ListPagedResponse<RecepcionistaResponse>>
{
    public async Task<ListPagedResponse<RecepcionistaResponse>> Handle(SearchRecepcionistaQuery request, CancellationToken cancellationToken)
    {
        var possuiSituacaoValida = SituacaoGeralHelper.TryParse(request.Situacao, out var situacao);

        Expression<Func<Recepcionista, bool>> filtro = recepcionista =>
            (string.IsNullOrWhiteSpace(request.Nome) || recepcionista.Nome.ToLower().Contains(request.Nome.ToLower())) &&
            (string.IsNullOrWhiteSpace(request.Email) || recepcionista.Email.Contains(request.Email.NormalizarEmail())) &&
            (string.IsNullOrWhiteSpace(request.Situacao) || (possuiSituacaoValida && recepcionista.Situacao == situacao));

        var total = await recepcionistaRepository.CountAsync(filtro, cancellationToken);
        var recepcionistas = await recepcionistaRepository.SearchAsync(
            filtro,
            Ordenar(request),
            PaginacaoHelper.CalcularSkip(request),
            PaginacaoHelper.NormalizarQuantidade(request.QuantidadeRegistros),
            cancellationToken);

        return PaginacaoHelper.CriarResposta(_mapper.Map<List<RecepcionistaResponse>>(recepcionistas), request, total);
    }

    private static Func<IQueryable<Recepcionista>, IOrderedQueryable<Recepcionista>> Ordenar(SearchRecepcionistaQuery request)
    {
        var desc = request.DirectionOrder == DirecaoOrdenacao.Desc;

        return request.OrderBy?.ToLowerInvariant() switch
        {
            "email" => query => desc ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email),
            _ => query => desc ? query.OrderByDescending(x => x.Nome) : query.OrderBy(x => x.Nome)
        };
    }
}
