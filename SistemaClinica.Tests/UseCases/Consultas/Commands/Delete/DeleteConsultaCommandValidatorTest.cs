using SistemaClinica.Application.UseCases.Consultas.Commands.Delete;

namespace SistemaClinica.Tests.UseCases.Consultas.Commands.Delete;

public class DeleteConsultaCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Id_For_Informado()
    {
        var resultado = new DeleteConsultaCommandValidator().Validate(new DeleteConsultaCommand { Id = 1 });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var resultado = new DeleteConsultaCommandValidator().Validate(new DeleteConsultaCommand { Id = 0 });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador da consulta.");
    }
}
