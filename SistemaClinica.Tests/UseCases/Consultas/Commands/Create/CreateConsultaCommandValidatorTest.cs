using SistemaClinica.Application.UseCases.Consultas.Commands.Create;

namespace SistemaClinica.Tests.UseCases.Consultas.Commands.Create;

public class CreateConsultaCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var validator = new CreateConsultaCommandValidator();
        var command = new CreateConsultaCommand { PacienteId = 1, MedicoId = 1, EspecialidadeId = 1, DataConsulta = DateTime.Now.AddDays(1) };

        var resultado = validator.Validate(command);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Medico_Nao_For_Informado()
    {
        var validator = new CreateConsultaCommandValidator();
        var command = new CreateConsultaCommand { PacienteId = 1, MedicoId = 0, EspecialidadeId = 1, DataConsulta = DateTime.Now.AddDays(1) };

        var resultado = validator.Validate(command);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o médico da consulta.");
    }
}
