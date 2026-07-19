using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Create;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Commands.Create;

public class CreateRecepcionistaCommandHandlerTest
{
    [Fact]
    public async Task Deve_Criar_Recepcionista_Quando_Email_Nao_Existir()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        var senhaService = new Mock<ISenhaService>();
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Recepcionista, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        senhaService.Setup(x => x.Criptografar("12345678")).Returns("hash");

        var response = await new CreateRecepcionistaCommandHandler(repository.Object, senhaService.Object, MapperHelper.Mapper)
            .Handle(new CreateRecepcionistaCommand { Nome = "A", Email = "a@clinica.com", Senha = "12345678" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("A", response.Data!.Nome);
        repository.Verify(x => x.InsertAsync(It.IsAny<Recepcionista>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Email_Ja_Existir()
    {
        var repository = new Mock<IRepository<Recepcionista>>();
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Recepcionista, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var response = await new CreateRecepcionistaCommandHandler(repository.Object, new Mock<ISenhaService>().Object, MapperHelper.Mapper)
            .Handle(new CreateRecepcionistaCommand { Nome = "A", Email = "a@clinica.com", Senha = "12345678" }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Já existe um recepcionista cadastrado com este e-mail.", response.Errors);
    }
}
