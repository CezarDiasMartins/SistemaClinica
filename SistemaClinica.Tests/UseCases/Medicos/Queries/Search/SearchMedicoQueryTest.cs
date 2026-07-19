using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Medicos.Queries.Search;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Medicos.Queries.Search;

public class SearchMedicoQueryTest
{
    [Fact]
    public async Task Deve_Buscar_Medicos_Com_Filtros()
    {
        var repository = new Mock<IRepository<Medico>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Medico, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Medico, bool>>>(), It.IsAny<Func<IQueryable<Medico>, IOrderedQueryable<Medico>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Medico, object>>[]>()))
            .ReturnsAsync([new Medico { Id = 1, Nome = "Dr. A", EspecialidadeId = 1, Especialidade = new Especialidade { Id = 1, Descricao = "Cardiologia" } }]);

        var response = await new SearchMedicoQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchMedicoQuery { EspecialidadeId = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Registros);
    }

    [Fact]
    public async Task Deve_Retornar_Vazio_Quando_Nao_Encontrar_Medicos()
    {
        var repository = new Mock<IRepository<Medico>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Medico, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Medico, bool>>>(), It.IsAny<Func<IQueryable<Medico>, IOrderedQueryable<Medico>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Medico, object>>[]>()))
            .ReturnsAsync([]);

        var response = await new SearchMedicoQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchMedicoQuery { Nome = "Nada" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Registros);
    }
}
