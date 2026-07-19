using System.Linq.Expressions;
using MediatR;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Request;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Consultas.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Consultas.Queries.Search;

public class SearchConsultaQuery : ListPagedRequest, IRequest<ListPagedResponse<ConsultaResponse>>
{
    private DateTime? _dataInicial;
    private DateTime? _dataFinal;

    public int? PacienteId { get; set; }
    public int? MedicoId { get; set; }
    public int? EspecialidadeId { get; set; }
    public DateTime? DataInicial
    {
        get => _dataInicial;
        set => _dataInicial = DataHoraHelper.ParaLocalSemFuso(value);
    }
    public DateTime? DataFinal
    {
        get => _dataFinal;
        set => _dataFinal = DataHoraHelper.ParaLocalSemFuso(value);
    }
    public SituacaoConsulta? SituacaoConsulta { get; set; }
}

public class SearchConsultaQueryHandler(IRepository<Consulta> consultaRepository, IMapper _mapper)
    : IRequestHandler<SearchConsultaQuery, ListPagedResponse<ConsultaResponse>>
{
    public async Task<ListPagedResponse<ConsultaResponse>> Handle(SearchConsultaQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Consulta, bool>> filtro = consulta =>
            (!request.PacienteId.HasValue || consulta.PacienteId == request.PacienteId.Value) &&
            (!request.MedicoId.HasValue || consulta.MedicoId == request.MedicoId.Value) &&
            (!request.EspecialidadeId.HasValue || consulta.EspecialidadeId == request.EspecialidadeId.Value) &&
            (!request.DataInicial.HasValue || consulta.DataConsulta >= request.DataInicial.Value) &&
            (!request.DataFinal.HasValue || consulta.DataConsulta <= request.DataFinal.Value) &&
            (!request.SituacaoConsulta.HasValue || consulta.SituacaoConsulta == request.SituacaoConsulta.Value);

        var total = await consultaRepository.CountAsync(filtro, cancellationToken);
        var consultas = await consultaRepository.SearchAsync(
            filtro,
            Ordenar(request),
            PaginacaoHelper.CalcularSkip(request),
            PaginacaoHelper.NormalizarQuantidade(request.QuantidadeRegistros),
            cancellationToken,
            c => c.Paciente!,
            c => c.Medico!,
            c => c.Especialidade!);

        return PaginacaoHelper.CriarResposta(_mapper.Map<List<ConsultaResponse>>(consultas), request, total);
    }

    private static Func<IQueryable<Consulta>, IOrderedQueryable<Consulta>> Ordenar(SearchConsultaQuery request)
    {
        var desc = request.DirectionOrder == DirecaoOrdenacao.Desc;
        return request.OrderBy?.ToLowerInvariant() switch
        {
            "medico" => query => desc ? query.OrderByDescending(x => x.Medico!.Nome) : query.OrderBy(x => x.Medico!.Nome),
            "paciente" => query => desc ? query.OrderByDescending(x => x.Paciente!.Nome) : query.OrderBy(x => x.Paciente!.Nome),
            "especialidade" => query => desc ? query.OrderByDescending(x => x.Especialidade!.Descricao) : query.OrderBy(x => x.Especialidade!.Descricao),
            _ => query => desc ? query.OrderByDescending(x => x.DataConsulta) : query.OrderBy(x => x.DataConsulta)
        };
    }
}
