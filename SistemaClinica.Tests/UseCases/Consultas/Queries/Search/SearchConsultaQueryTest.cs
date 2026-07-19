using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Consultas.Queries.Search;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Consultas.Queries.Search;

public class SearchConsultaQueryTest
{
    [Fact]
    public async Task Deve_Buscar_Consultas_Com_Filtros()
    {
        var repository = new Mock<IRepository<Consulta>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<Func<IQueryable<Consulta>, IOrderedQueryable<Consulta>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Consulta, object>>[]>()))
            .ReturnsAsync([new Consulta { Id = 1, EspecialidadeId = 1, Especialidade = new Especialidade { Id = 1, Descricao = "Cardiologia" } }]);

        var response = await new SearchConsultaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchConsultaQuery { EspecialidadeId = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Registros);
        Assert.Equal(1, response.TotalRegistros);
    }

    [Fact]
    public async Task Deve_Retornar_Vazio_Quando_Nao_Encontrar_Consultas()
    {
        var repository = new Mock<IRepository<Consulta>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<Func<IQueryable<Consulta>, IOrderedQueryable<Consulta>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Consulta, object>>[]>()))
            .ReturnsAsync([]);

        var response = await new SearchConsultaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchConsultaQuery { EspecialidadeId = 99 }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Registros);
    }
}
