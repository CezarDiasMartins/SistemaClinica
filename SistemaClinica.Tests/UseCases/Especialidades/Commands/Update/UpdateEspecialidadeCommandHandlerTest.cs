using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Especialidades.Commands.Update;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Especialidades.Commands.Update;

public class UpdateEspecialidadeCommandHandlerTest
{
    [Fact]
    public async Task Deve_Atualizar_Especialidade_Quando_Dados_Forem_Validos()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Especialidade { Id = 1, Descricao = "Clínica" });
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Especialidade, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var response = await new UpdateEspecialidadeCommandHandler(repository.Object, MapperHelper.Mapper)
            .Handle(new UpdateEspecialidadeCommand { Id = 1, Descricao = "Cardiologia" }, CancellationToken.None);

        Assert.True(response.Success);
        repository.Verify(x => x.Update(It.IsAny<Especialidade>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Especialidade_Nao_Existir()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Especialidade?)null);

        var response = await new UpdateEspecialidadeCommandHandler(repository.Object, MapperHelper.Mapper)
            .Handle(new UpdateEspecialidadeCommand { Id = 1, Descricao = "Cardiologia" }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Especialidade não encontrada.", response.Errors);
    }
}
