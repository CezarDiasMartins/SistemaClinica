using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Pacientes.Commands.Update;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Tests.UseCases.Pacientes.Commands.Update;

public class UpdatePacienteCommandHandlerTest
{
    [Fact]
    public async Task Deve_Atualizar_Paciente_Quando_Dados_Forem_Validos()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Paciente { Id = 1, CPF = "12345678901" });
        repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Paciente, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var response = await new UpdatePacienteCommandHandler(repository.Object, MapperHelper.Mapper).Handle(CriarCommandValido(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("A", response.Data!.Situacao);
        repository.Verify(x => x.Update(It.Is<Paciente>(paciente => paciente.Situacao == SituacaoGeral.Ativo)), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Paciente_Nao_Existir()
    {
        var repository = new Mock<IRepository<Paciente>>();
        repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Paciente?)null);

        var response = await new UpdatePacienteCommandHandler(repository.Object, MapperHelper.Mapper).Handle(CriarCommandValido(), CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Paciente não encontrado.", response.Errors);
    }

    private static UpdatePacienteCommand CriarCommandValido()
    {
        return new UpdatePacienteCommand { Id = 1, Nome = "A", CPF = "12345678901", DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)), Telefone = "67999999999", Email = "a@teste.com", Sexo = Sexo.Feminino, Situacao = "A" };
    }
}
