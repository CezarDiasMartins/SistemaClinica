using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Especialidades.Commands.Delete;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Especialidades.Commands.Delete;

public class DeleteEspecialidadeCommandHandlerTest
{
    [Fact]
    public async Task Deve_Deletar_Especialidade_Quando_Existir()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Especialidade { Id = 1 });

        var response = await new DeleteEspecialidadeCommandHandler(repository.Object)
            .Handle(new DeleteEspecialidadeCommand { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        repository.Verify(x => x.Delete(It.IsAny<Especialidade>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Especialidade_Nao_Existir()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Especialidade?)null);

        var response = await new DeleteEspecialidadeCommandHandler(repository.Object)
            .Handle(new DeleteEspecialidadeCommand { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Especialidade não encontrada.", response.Errors);
    }
}
