using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Update;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Commands.Update;

public class UpdateRecepcionistaCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var command = new UpdateRecepcionistaCommand { Id = 1, Nome = "A", Email = "a@clinica.com", Situacao = "A" };

        var resultado = new UpdateRecepcionistaCommandValidator().Validate(command);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var command = new UpdateRecepcionistaCommand { Id = 0, Nome = "A", Email = "a@clinica.com", Situacao = "A" };

        var resultado = new UpdateRecepcionistaCommandValidator().Validate(command);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador do recepcionista.");
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Situacao_For_Invalida()
    {
        var command = new UpdateRecepcionistaCommand { Id = 1, Nome = "A", Email = "a@clinica.com", Situacao = "X" };

        var resultado = new UpdateRecepcionistaCommandValidator().Validate(command);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe uma situação válida para o recepcionista.");
    }
}
