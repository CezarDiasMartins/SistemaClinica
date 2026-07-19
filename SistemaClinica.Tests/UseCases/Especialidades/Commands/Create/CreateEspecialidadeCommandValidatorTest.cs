using SistemaClinica.Application.UseCases.Especialidades.Commands.Create;

namespace SistemaClinica.Tests.UseCases.Especialidades.Commands.Create;

public class CreateEspecialidadeCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var resultado = new CreateEspecialidadeCommandValidator().Validate(new CreateEspecialidadeCommand { Descricao = "Cardiologia" });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Descricao_Nao_For_Informada()
    {
        var resultado = new CreateEspecialidadeCommandValidator().Validate(new CreateEspecialidadeCommand { Descricao = "" });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe a descrição da especialidade.");
    }
}
