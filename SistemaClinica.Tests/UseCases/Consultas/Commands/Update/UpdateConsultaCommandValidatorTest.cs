using SistemaClinica.Application.UseCases.Consultas.Commands.Update;

namespace SistemaClinica.Tests.UseCases.Consultas.Commands.Update;

public class UpdateConsultaCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var validator = new UpdateConsultaCommandValidator();
        var command = new UpdateConsultaCommand { Id = 1, PacienteId = 1, MedicoId = 1, EspecialidadeId = 1, DataConsulta = DateTime.Now.AddDays(1) };

        var resultado = validator.Validate(command);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var validator = new UpdateConsultaCommandValidator();
        var command = new UpdateConsultaCommand { Id = 0, PacienteId = 1, MedicoId = 1, EspecialidadeId = 1, DataConsulta = DateTime.Now.AddDays(1) };

        var resultado = validator.Validate(command);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador da consulta.");
    }
}
