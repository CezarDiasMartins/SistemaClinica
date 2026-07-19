using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Pacientes.Queries.List;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Pacientes.Queries.List;

public class ListPacienteQueryTest
{
    [Fact]
    public async Task Deve_Listar_Pacientes()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([new Paciente { Id = 1 }]);

        var response = await new ListPacienteQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListPacienteQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Data!);
    }

    [Fact]
    public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Pacientes()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var response = await new ListPacienteQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new ListPacienteQuery(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Data!);
    }
}
