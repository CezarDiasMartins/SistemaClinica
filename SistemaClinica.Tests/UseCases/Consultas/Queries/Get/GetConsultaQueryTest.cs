using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Consultas.Queries.Get;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Consultas.Queries.Get;

public class GetConsultaQueryTest
{
    [Fact]
    public async Task Deve_Retornar_Consulta_Quando_Existir()
    {
        var repository = new Mock<IRepository<Consulta>>();
        repository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Consulta, object>>[]>()))
            .ReturnsAsync(new Consulta { Id = 1, Paciente = new Paciente { Nome = "Nome" }, Medico = new Medico { Nome = "Dr. A" } });

        var response = await new GetConsultaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetConsultaQuery { Id = 1 }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal(1, response.Data!.Id);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Consulta_Nao_Existir()
    {
        var repository = new Mock<IRepository<Consulta>>();
        repository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Consulta, object>>[]>()))
            .ReturnsAsync((Consulta?)null);

        var response = await new GetConsultaQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new GetConsultaQuery { Id = 1 }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Consulta não encontrada.", response.Errors);
    }
}
