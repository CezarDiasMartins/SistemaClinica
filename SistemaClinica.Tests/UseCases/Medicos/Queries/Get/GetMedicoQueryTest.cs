using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Medicos.Queries.Get;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Medicos.Queries.Get;

public class GetMedicoQueryTest
{
    [Fact]
    public async Task Deve_Retornar_Medico_Quando_Existir()
    {
        var repository = new Mock<IRepository<Medico>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Medico { Id = 1, Nome = "Dr. A" });

        var response = await new GetMedicoQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetMedicoQuery { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("Dr. A", response.Data!.Nome);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Medico_Nao_Existir()
    {
        var repository = new Mock<IRepository<Medico>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Medico?)null);

        var response = await new GetMedicoQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetMedicoQuery { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Médico não encontrado.", response.Errors);
    }
}
