using SistemaClinica.Application.UseCases.Medicos.Commands.Create;

namespace SistemaClinica.Tests.UseCases.Medicos.Commands.Create;

public class CreateMedicoCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var resultado = new CreateMedicoCommandValidator().Validate(new CreateMedicoCommand { Nome = "Dra. A", CRM = "CRM123", EspecialidadeId = 1, Telefone = "67999999999", Email = "a@clinica.com" });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_CRM_Nao_For_Informado()
    {
        var resultado = new CreateMedicoCommandValidator().Validate(new CreateMedicoCommand { Nome = "Dra. A", CRM = "", EspecialidadeId = 1, Telefone = "67999999999", Email = "a@clinica.com" });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o CRM do médico.");
    }
}
