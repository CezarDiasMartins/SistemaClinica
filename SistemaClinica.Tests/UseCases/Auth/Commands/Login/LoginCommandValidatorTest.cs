using SistemaClinica.Application.UseCases.Auth.Commands.Login;

namespace SistemaClinica.Tests.UseCases.Auth.Commands.Login;

public class LoginCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var validator = new LoginCommandValidator();
        var command = new LoginCommand { Email = "admin@clinica.com", Senha = "12345678" };

        var resultado = validator.Validate(command);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Email_For_Invalido()
    {
        var validator = new LoginCommandValidator();
        var command = new LoginCommand { Email = "email-invalido", Senha = "" };

        var resultado = validator.Validate(command);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe um e-mail válido.");
    }
}
