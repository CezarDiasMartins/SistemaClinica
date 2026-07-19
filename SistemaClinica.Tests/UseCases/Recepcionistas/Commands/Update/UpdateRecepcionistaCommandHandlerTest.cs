using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Update;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Commands.Update;

public class UpdateRecepcionistaCommandHandlerTest
{
    [Fact]
    public async Task Deve_Atualizar_Recepcionista_Quando_Dados_Forem_Validos()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Recepcionista { Id = 1, Email = "a@clinica.com" });
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Recepcionista, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var response = await new UpdateRecepcionistaCommandHandler(repository.Object, new Mock<ISenhaService>().Object, MapperHelper.Mapper)
            .Handle(new UpdateRecepcionistaCommand { Id = 1, Nome = "A", Email = "a@clinica.com", Situacao = "A" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("A", response.Data!.Situacao);
        repository.Verify(x => x.Update(It.Is<Recepcionista>(recepcionista => recepcionista.Situacao == SituacaoGeral.Ativo)), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Recepcionista_Nao_Existir()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Recepcionista?)null);

        var response = await new UpdateRecepcionistaCommandHandler(repository.Object, new Mock<ISenhaService>().Object, MapperHelper.Mapper)
            .Handle(new UpdateRecepcionistaCommand { Id = 1, Nome = "A", Email = "a@clinica.com", Situacao = "A" }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Recepcionista não encontrado.", response.Errors);
    }
}
