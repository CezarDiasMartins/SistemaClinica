using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Pacientes.Commands.Delete;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Pacientes.Commands.Delete;

public class DeletePacienteCommandHandlerTest
{
    [Fact]
    public async Task Deve_Deletar_Paciente_Quando_Existir()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Paciente { Id = 1 });

        var response = await new DeletePacienteCommandHandler(repository.Object).Handle(new DeletePacienteCommand { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        repository.Verify(x => x.Delete(It.IsAny<Paciente>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Paciente_Nao_Existir()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Paciente?)null);

        var response = await new DeletePacienteCommandHandler(repository.Object).Handle(new DeletePacienteCommand { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Paciente não encontrado.", response.Errors);
    }
}
