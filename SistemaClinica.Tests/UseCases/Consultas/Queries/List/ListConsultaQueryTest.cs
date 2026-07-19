using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Consultas.Queries.List;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Consultas.Queries.List;

public class ListConsultaQueryTest
{
    [Fact]
    public async Task Deve_Listar_Consultas()
    {
        var repository = new Mock<IRepository<Consulta>>();
        repository.Setup(x => x.SearchAsync(null, It.IsAny<Func<IQueryable<Consulta>, IOrderedQueryable<Consulta>>>(), 0, 500, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Consulta, object>>[]>()))
            .ReturnsAsync([new Consulta { Id = 1 }]);

        var response = await new ListConsultaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListConsultaQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Data!);
    }

    [Fact]
    public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Consultas()
    {
        var repository = new Mock<IRepository<Consulta>>();
        repository.Setup(x => x.SearchAsync(null, It.IsAny<Func<IQueryable<Consulta>, IOrderedQueryable<Consulta>>>(), 0, 500, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Consulta, object>>[]>()))
            .ReturnsAsync([]);

        var response = await new ListConsultaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListConsultaQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Data!);
    }
}
