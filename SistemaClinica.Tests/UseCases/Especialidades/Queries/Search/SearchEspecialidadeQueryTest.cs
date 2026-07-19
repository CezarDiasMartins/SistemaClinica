using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Especialidades.Queries.Search;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Especialidades.Queries.Search;

public class SearchEspecialidadeQueryTest
{
    [Fact]
    public async Task Deve_Buscar_Especialidades_Com_Filtros()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Especialidade, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Especialidade, bool>>>(), It.IsAny<Func<IQueryable<Especialidade>, IOrderedQueryable<Especialidade>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Especialidade, object>>[]>()))
            .ReturnsAsync([new Especialidade { Id = 1, Descricao = "Cardiologia" }]);

        var response = await new SearchEspecialidadeQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchEspecialidadeQuery { Descricao = "Cardio" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Registros);
    }

    [Fact]
    public async Task Deve_Retornar_Vazio_Quando_Nao_Encontrar_Especialidades()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Especialidade, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Especialidade, bool>>>(), It.IsAny<Func<IQueryable<Especialidade>, IOrderedQueryable<Especialidade>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Especialidade, object>>[]>()))
            .ReturnsAsync([]);

        var response = await new SearchEspecialidadeQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchEspecialidadeQuery { Descricao = "Nada" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Registros);
    }
}
