using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Auth.Commands.Login;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Auth.Commands.Login;

public class LoginCommandHandlerTest
{
    [Fact]
    public async Task Deve_Retornar_Token_Quando_Credenciais_Forem_Validas()
    {
        var usuarioRepository = new Mock<IRepository<Usuario>>();
        var senhaService = new Mock<ISenhaService>();
        var tokenService = new Mock<ITokenService>();
        var usuario = new Administrador { Id = 1, Nome = "Admin", Email = "admin@clinica.com", Senha = "hash" };

        usuarioRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        senhaService.Setup(x => x.Verificar("12345678", "hash")).Returns(true);
        tokenService.Setup(x => x.GerarToken(usuario)).Returns("token");

        var handler = new LoginCommandHandler(usuarioRepository.Object, senhaService.Object, tokenService.Object);
        var response = await handler.Handle(new LoginCommand { Email = "ADMIN@CLINICA.COM", Senha = "12345678" }, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("token", response.Data!.Token);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Credenciais_Forem_Invalidas()
    {
        var usuarioRepository = new Mock<IRepository<Usuario>>();
        var senhaService = new Mock<ISenhaService>();
        var tokenService = new Mock<ITokenService>();

        usuarioRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        var handler = new LoginCommandHandler(usuarioRepository.Object, senhaService.Object, tokenService.Object);
        var response = await handler.Handle(new LoginCommand { Email = "admin@clinica.com", Senha = "errada" }, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("E-mail ou senha inválidos.", response.Errors);
    }
}
