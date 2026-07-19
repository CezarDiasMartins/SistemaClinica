using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Recepcionistas.Queries.Get;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Queries.Get;

public class GetRecepcionistaQueryTest
{
    [Fact]
    public async Task Deve_Retornar_Recepcionista_Quando_Existir()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Recepcionista { Id = 1, Nome = "Ana" });

        var response = await new GetRecepcionistaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetRecepcionistaQuery { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("Ana", response.Data!.Nome);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Recepcionista_Nao_Existir()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Recepcionista?)null);

        var response = await new GetRecepcionistaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetRecepcionistaQuery { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Recepcionista não encontrado.", response.Errors);
    }
}
