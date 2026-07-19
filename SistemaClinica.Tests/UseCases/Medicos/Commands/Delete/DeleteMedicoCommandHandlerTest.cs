using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Medicos.Commands.Delete;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Medicos.Commands.Delete;

public class DeleteMedicoCommandHandlerTest
{
    [Fact]
    public async Task Deve_Deletar_Medico_Quando_Existir()
    {
        var repository = new Mock<IRepository<Medico>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Medico { Id = 1 });

        var response = await new DeleteMedicoCommandHandler(repository.Object).Handle(new DeleteMedicoCommand { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        repository.Verify(x => x.Delete(It.IsAny<Medico>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Medico_Nao_Existir()
    {
        var repository = new Mock<IRepository<Medico>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Medico?)null);

        var response = await new DeleteMedicoCommandHandler(repository.Object).Handle(new DeleteMedicoCommand { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Médico não encontrado.", response.Errors);
    }
}
