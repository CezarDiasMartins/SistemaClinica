using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Recepcionistas.Queries.List;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Queries.List;

public class ListRecepcionistaQueryTest
{
    [Fact]
    public async Task Deve_Listar_Recepcionistas()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([new Recepcionista { Id = 1 }]);

        var response = await new ListRecepcionistaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListRecepcionistaQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Data!);
    }

    [Fact]
    public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Recepcionistas()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var response = await new ListRecepcionistaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListRecepcionistaQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Data!);
    }
}
