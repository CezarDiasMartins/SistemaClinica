using SistemaClinica.Application.UseCases.Medicos.Commands.Delete;

namespace SistemaClinica.Tests.UseCases.Medicos.Commands.Delete;

public class DeleteMedicoCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Id_For_Informado()
    {
        var resultado = new DeleteMedicoCommandValidator().Validate(new DeleteMedicoCommand { Id = 1 });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var resultado = new DeleteMedicoCommandValidator().Validate(new DeleteMedicoCommand { Id = 0 });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador do médico.");
    }
}
