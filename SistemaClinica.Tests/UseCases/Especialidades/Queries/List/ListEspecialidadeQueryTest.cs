using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Especialidades.Queries.List;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Especialidades.Queries.List;

public class ListEspecialidadeQueryTest
{
    [Fact]
    public async Task Deve_Listar_Especialidades()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([new Especialidade { Id = 1 }]);

        var response = await new ListEspecialidadeQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListEspecialidadeQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Data!);
    }

    [Fact]
    public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Especialidades()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var response = await new ListEspecialidadeQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListEspecialidadeQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Data!);
    }
}
