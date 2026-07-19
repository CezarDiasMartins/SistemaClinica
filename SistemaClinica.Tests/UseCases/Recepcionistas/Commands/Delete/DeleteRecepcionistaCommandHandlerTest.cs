using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Delete;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Commands.Delete;

public class DeleteRecepcionistaCommandHandlerTest
{
    [Fact]
    public async Task Deve_Deletar_Recepcionista_Quando_Existir()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Recepcionista { Id = 1 });

        var response = await new DeleteRecepcionistaCommandHandler(repository.Object).Handle(new DeleteRecepcionistaCommand { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        repository.Verify(x => x.Delete(It.IsAny<Recepcionista>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Recepcionista_Nao_Existir()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Recepcionista?)null);

        var response = await new DeleteRecepcionistaCommandHandler(repository.Object).Handle(new DeleteRecepcionistaCommand { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Recepcionista não encontrado.", response.Errors);
    }
}
