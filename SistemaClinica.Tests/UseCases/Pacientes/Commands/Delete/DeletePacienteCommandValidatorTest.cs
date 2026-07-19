using SistemaClinica.Application.UseCases.Pacientes.Commands.Delete;

namespace SistemaClinica.Tests.UseCases.Pacientes.Commands.Delete;

public class DeletePacienteCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Id_For_Informado()
    {
        var resultado = new DeletePacienteCommandValidator().Validate(new DeletePacienteCommand { Id = 1 });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var resultado = new DeletePacienteCommandValidator().Validate(new DeletePacienteCommand { Id = 0 });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador do paciente.");
    }
}
