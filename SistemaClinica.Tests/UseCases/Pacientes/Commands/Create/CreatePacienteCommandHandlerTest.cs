using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Pacientes.Commands.Create;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Tests.UseCases.Pacientes.Commands.Create;

public class CreatePacienteCommandHandlerTest
{
    [Fact]
    public async Task Deve_Criar_Paciente_Quando_CPF_Nao_Existir()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Paciente, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var response = await new CreatePacienteCommandHandler(repository.Object, MapperHelper.Mapper).Handle(CriarCommandValido(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal(RoleUsuario.Paciente, response.Data!.Role);
        repository.Verify(x => x.InsertAsync(It.IsAny<Paciente>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_CPF_Ja_Existir()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Paciente, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var response = await new CreatePacienteCommandHandler(repository.Object, MapperHelper.Mapper).Handle(CriarCommandValido(), CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Já existe um paciente cadastrado com este CPF.", response.Errors);
    }

    private static CreatePacienteCommand CriarCommandValido()
    {
        return new CreatePacienteCommand { Nome = "A", CPF = "12345678901", DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)), Telefone = "67999999999", Email = "a@teste.com", Sexo = Sexo.Masculino };
    }
}
