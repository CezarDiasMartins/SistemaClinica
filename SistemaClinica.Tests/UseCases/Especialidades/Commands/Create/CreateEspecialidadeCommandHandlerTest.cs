using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Especialidades.Commands.Create;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Especialidades.Commands.Create;

public class CreateEspecialidadeCommandHandlerTest
{
    [Fact]
    public async Task Deve_Criar_Especialidade_Quando_Descricao_Nao_Existir()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Especialidade, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var response = await new CreateEspecialidadeCommandHandler(repository.Object, MapperHelper.Mapper)
            .Handle(new CreateEspecialidadeCommand { Descricao = "Cardiologia" }, CancellationToken.None);

        Assert.True(response.Success);
        repository.Verify(x => x.InsertAsync(It.IsAny<Especialidade>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Descricao_Ja_Existir()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Especialidade, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var response = await new CreateEspecialidadeCommandHandler(repository.Object, MapperHelper.Mapper)
            .Handle(new CreateEspecialidadeCommand { Descricao = "Cardiologia" }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Já existe uma especialidade cadastrada com esta descrição.", response.Errors);
    }
}
