using SistemaClinica.Application.UseCases.Pacientes.Commands.Update;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Tests.UseCases.Pacientes.Commands.Update;

public class UpdatePacienteCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var resultado = new UpdatePacienteCommandValidator().Validate(CriarCommandValido());

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var command = CriarCommandValido();
        command.Id = 0;

        var resultado = new UpdatePacienteCommandValidator().Validate(command);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador do paciente.");
    }

    private static UpdatePacienteCommand CriarCommandValido()
    {
        return new UpdatePacienteCommand { Id = 1, Nome = "A", CPF = "12345678901", DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)), Telefone = "67999999999", Email = "a@teste.com", Sexo = Sexo.Feminino, Situacao = "A" };
    }
}
