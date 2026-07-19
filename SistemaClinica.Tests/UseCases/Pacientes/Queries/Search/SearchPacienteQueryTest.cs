using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Pacientes.Queries.Search;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Pacientes.Queries.Search;

public class SearchPacienteQueryTest
{
    [Fact]
    public async Task Deve_Buscar_Pacientes_Com_Filtros()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Paciente, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Paciente, bool>>>(), It.IsAny<Func<IQueryable<Paciente>, IOrderedQueryable<Paciente>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Paciente, object>>[]>()))
            .ReturnsAsync([new Paciente { Id = 1, Nome = "Maria" }]);

        var response = await new SearchPacienteQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchPacienteQuery { Nome = "Maria" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Single(response.Registros);
    }

    [Fact]
    public async Task Deve_Retornar_Vazio_Quando_Nao_Encontrar_Pacientes()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Paciente, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
        repository.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Paciente, bool>>>(), It.IsAny<Func<IQueryable<Paciente>, IOrderedQueryable<Paciente>>>(), 0, 10, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Paciente, object>>[]>()))
            .ReturnsAsync([]);

        var response = await new SearchPacienteQueryHandler(repository.Object, MapperHelper.Mapper).Handle(new SearchPacienteQuery { Nome = "Nada" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Empty(response.Registros);
    }
}
