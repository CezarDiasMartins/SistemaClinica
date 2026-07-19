using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Delete;

namespace SistemaClinica.Tests.UseCases.Recepcionistas.Commands.Delete;

public class DeleteRecepcionistaCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Id_For_Informado()
    {
        var resultado = new DeleteRecepcionistaCommandValidator().Validate(new DeleteRecepcionistaCommand { Id = 1 });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var resultado = new DeleteRecepcionistaCommandValidator().Validate(new DeleteRecepcionistaCommand { Id = 0 });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador do recepcionista.");
    }
}
