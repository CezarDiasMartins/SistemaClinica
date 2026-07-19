using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Consultas.Commands.Delete;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Consultas.Commands.Delete;

public class DeleteConsultaCommandHandlerTest
{
    [Fact]
    public async Task Deve_Deletar_Consulta_Quando_Existir()
    {
        var repository = new Mock<IRepository<Consulta>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Consulta { Id = 1 });

        var handler = new DeleteConsultaCommandHandler(repository.Object);
        var response = await handler.Handle(new DeleteConsultaCommand { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        repository.Verify(x => x.Delete(It.IsAny<Consulta>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Consulta_Nao_Existir()
    {
        var repository = new Mock<IRepository<Consulta>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Consulta?)null);

        var handler = new DeleteConsultaCommandHandler(repository.Object);
        var response = await handler.Handle(new DeleteConsultaCommand { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Consulta não encontrada.", response.Errors);
    }
}
