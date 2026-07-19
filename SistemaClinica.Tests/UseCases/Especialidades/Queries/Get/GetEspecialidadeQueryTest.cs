using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Especialidades.Queries.Get;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Especialidades.Queries.Get;

public class GetEspecialidadeQueryTest
{
    [Fact]
    public async Task Deve_Retornar_Especialidade_Quando_Existir()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Especialidade { Id = 1, Descricao = "Cardiologia" });

        var response = await new GetEspecialidadeQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetEspecialidadeQuery { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("Cardiologia", response.Data!.Descricao);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Especialidade_Nao_Existir()
    {
        var repository = new Mock<IRepository<Especialidade>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Especialidade?)null);

        var response = await new GetEspecialidadeQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetEspecialidadeQuery { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Especialidade não encontrada.", response.Errors);
    }
}
