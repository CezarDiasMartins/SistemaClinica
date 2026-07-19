using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Recepcionistas.Queries.Search;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Queries.Search;

public class SearchRecepcionistaQueryTest
{
    [Fact]
    public async Task Deve_Buscar_Recepcionistas_Com_Filtros()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Recepcionista, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Recepcionista, bool>>>(), It.IsAny<Func<IQueryable<Recepcionista>, IOrderedQueryable<Recepcionista>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Recepcionista, object>>[]>()))
            .ReturnsAsync([new Recepcionista { Id = 1, Nome = "Ana" }]);

        var response = await new SearchRecepcionistaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchRecepcionistaQuery { Nome = "Ana" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Registros);
    }

    [Fact]
    public async Task Deve_Retornar_Vazio_Quando_Nao_Encontrar_Recepcionistas()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Recepcionista, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Recepcionista, bool>>>(), It.IsAny<Func<IQueryable<Recepcionista>, IOrderedQueryable<Recepcionista>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Recepcionista, object>>[]>()))
            .ReturnsAsync([]);

        var response = await new SearchRecepcionistaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchRecepcionistaQuery { Nome = "Nada" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Registros);
    }
}
