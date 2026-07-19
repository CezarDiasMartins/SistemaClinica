using System.Linq.Expressions;
using MediatR;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Request;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Pacientes.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;
using SistemaClinica.Libs.Extensions;

namespace SistemaClinica.Application.UseCases.Pacientes.Queries.Search;

public class SearchPacienteQuery : ListPagedRequest, IRequest<ListPagedResponse<PacienteResponse>>
{
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public Sexo? Sexo { get; set; }
    public string? Situacao { get; set; }
}

public class SearchPacienteQueryHandler(IRepository<Paciente> pacienteRepository, IMapper _mapper)
    : IRequestHandler<SearchPacienteQuery, ListPagedResponse<PacienteResponse>>
{
    public async Task<ListPagedResponse<PacienteResponse>> Handle(SearchPacienteQuery request, CancellationToken cancellationToken)
    {
        var cpf = request.CPF.SoNumeros();
        var possuiSituacaoValida = SituacaoGeralHelper.TryParse(request.Situacao, out var situacao);

        Expression<Func<Paciente, bool>> filtro = paciente =>
            (string.IsNullOrWhiteSpace(request.Nome) || paciente.Nome.ToLower().Contains(request.Nome.ToLower())) &&
            (string.IsNullOrWhiteSpace(cpf) || paciente.CPF.Contains(cpf)) &&
            (!request.Sexo.HasValue || paciente.Sexo == request.Sexo.Value) &&
            (string.IsNullOrWhiteSpace(request.Situacao) || (possuiSituacaoValida && paciente.Situacao == situacao));

        var total = await pacienteRepository.CountAsync(filtro, cancellationToken);
        var pacientes = await pacienteRepository.SearchAsync(filtro, Ordenar(request), PaginacaoHelper.CalcularSkip(request), PaginacaoHelper.NormalizarQuantidade(request.QuantidadeRegistros), cancellationToken);

        return PaginacaoHelper.CriarResposta(_mapper.Map<List<PacienteResponse>>(pacientes), request, total);
    }

    private static Func<IQueryable<Paciente>, IOrderedQueryable<Paciente>> Ordenar(SearchPacienteQuery request)
    {
        var desc = request.DirectionOrder == DirecaoOrdenacao.Desc;
        return request.OrderBy?.ToLowerInvariant() switch
        {
            "cpf" => query => desc ? query.OrderByDescending(x => x.CPF) : query.OrderBy(x => x.CPF),
            "datanascimento" => query => desc ? query.OrderByDescending(x => x.DataNascimento) : query.OrderBy(x => x.DataNascimento),
            _ => query => desc ? query.OrderByDescending(x => x.Nome) : query.OrderBy(x => x.Nome)
        };
    }
}
