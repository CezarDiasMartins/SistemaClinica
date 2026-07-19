using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Pacientes.Queries.Get;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Pacientes.Queries.Get;

public class GetPacienteQueryTest
{
    [Fact]
    public async Task Deve_Retornar_Paciente_Quando_Existir()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Paciente { Id = 1, Nome = "Maria" });

        var response = await new GetPacienteQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetPacienteQuery { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("Maria", response.Data!.Nome);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Paciente_Nao_Existir()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Paciente?)null);

        var response = await new GetPacienteQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetPacienteQuery { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Paciente não encontrado.", response.Errors);
    }
}
