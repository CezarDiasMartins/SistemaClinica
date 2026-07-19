using SistemaClinica.Application.UseCases.Especialidades.Commands.Update;

namespace SistemaClinica.Tests.UseCases.Especialidades.Commands.Update;

public class UpdateEspecialidadeCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Comando_For_Valido()
    {
        var resultado = new UpdateEspecialidadeCommandValidator().Validate(new UpdateEspecialidadeCommand { Id = 1, Descricao = "Cardiologia" });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var resultado = new UpdateEspecialidadeCommandValidator().Validate(new UpdateEspecialidadeCommand { Id = 0, Descricao = "Cardiologia" });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador da especialidade.");
    }
}
