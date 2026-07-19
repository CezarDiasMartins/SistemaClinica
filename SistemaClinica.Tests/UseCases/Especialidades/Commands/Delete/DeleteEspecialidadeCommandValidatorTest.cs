using SistemaClinica.Application.UseCases.Especialidades.Commands.Delete;

namespace SistemaClinica.Tests.UseCases.Especialidades.Commands.Delete;

public class DeleteEspecialidadeCommandValidatorTest
{
    [Fact]
    public void Deve_Validar_Quando_Id_For_Informado()
    {
        var resultado = new DeleteEspecialidadeCommandValidator().Validate(new DeleteEspecialidadeCommand { Id = 1 });

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Nao_For_Informado()
    {
        var resultado = new DeleteEspecialidadeCommandValidator().Validate(new DeleteEspecialidadeCommand { Id = 0 });

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, erro => erro.ErrorMessage == "Informe o identificador da especialidade.");
    }
}
