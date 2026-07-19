using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Medicos.Queries.List;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Medicos.Queries.List;

public class ListMedicoQueryTest
{
    [Fact]
    public async Task Deve_Listar_Medicos()
    {
        var repository = new Mock<IRepository<Medico>>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([new Medico { Id = 1 }]);

        var response = await new ListMedicoQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListMedicoQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Data!);
    }

    [Fact]
    public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Medicos()
    {
        var repository = new Mock<IRepository<Medico>>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var response = await new ListMedicoQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListMedicoQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Data!);
    }
}
