using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Create;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Commands.Create;

public class CreateRecepcionistaCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var command = new CreateRecepcionistaCommand { Nome = "A", Email = "a@clinica.com", Senha = "12345678" };

        var resultado = new CreateRecepcionistaCommandValidator().Validate(command);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Senha_For_Curta()
    {
        var command = new CreateRecepcionistaCommand { Nome = "A", Email = "a@clinica.com", Senha = "123" };

        var resultado = new CreateRecepcionistaCommandValidator().Validate(command);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "A senha do recepcionista deve ter pelo menos 8 caracteres.");
    }
}
