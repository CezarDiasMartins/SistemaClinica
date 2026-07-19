using SistemaClinica.Application.UseCases.Medicos.Commands.Update;

namespace SistemaClinica.Tests.UseCases.Medicos.Commands.Update;

public class UpdateMedicoCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var resultado = new UpdateMedicoCommandValidator().Validate(new UpdateMedicoCommand { Id = 1, Nome = "Dr. A", CRM = "CRM123", EspecialidadeId = 1, Telefone = "67999999999", Email = "a@clinica.com", Situacao = "A" });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var resultado = new UpdateMedicoCommandValidator().Validate(new UpdateMedicoCommand { Id = 0, Nome = "Dr. A", CRM = "CRM123", EspecialidadeId = 1, Telefone = "67999999999", Email = "a@clinica.com", Situacao = "A" });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador do médico.");
    }
}
