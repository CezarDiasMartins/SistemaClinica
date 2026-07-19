using SistemaClinica.Application.UseCases.Pacientes.Commands.Create;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Tests.UseCases.Pacientes.Commands.Create;

public class CreatePacienteCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var resultado = new CreatePacienteCommandValidator().Validate(CriarCommandValido());

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_CPF_For_Invalido()
    {
        var command = CriarCommandValido();
        command.CPF = "123";

        var resultado = new CreatePacienteCommandValidator().Validate(command);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe um CPF válido para o paciente.");
    }

    private static CreatePacienteCommand CriarCommandValido()
    {
        return new CreatePacienteCommand { Nome = "A", CPF = "12345678901", DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)), Telefone = "67999999999", Email = "a@teste.com", Sexo = Sexo.Feminino };
    }
}
