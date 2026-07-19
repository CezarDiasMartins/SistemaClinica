using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Medicos.Commands.Update;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Tests.UseCases.Medicos.Commands.Update;

public class UpdateMedicoCommandHandlerTest
{
    [Fact]
    public async Task Deve_Atualizar_Medico_Quando_Dados_Forem_Validos()
    {
        var medicoRepository = new Mock<IRepository<Medico>>();
        var especialidadeRepository = new Mock<IRepository<Especialidade>>();
        medicoRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Medico { Id = 1, CRM = "CRM1" });
        medicoRepository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Medico, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        especialidadeRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Especialidade { Id = 1, Descricao = "Cardiologia" });

        var response = await new UpdateMedicoCommandHandler(medicoRepository.Object, especialidadeRepository.Object, MapperHelper.Mapper)
            .Handle(CriarCommandValido(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal("A", response.Data!.Situacao);
        medicoRepository.Verify(x => x.Update(It.Is<Medico>(medico => medico.Situacao == SituacaoGeral.Ativo)), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Medico_Nao_Existir()
    {
        var medicoRepository = new Mock<IRepository<Medico>>();
        medicoRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Medico?)null);

        var response = await new UpdateMedicoCommandHandler(medicoRepository.Object, new Mock<IRepository<Especialidade>>().Object, MapperHelper.Mapper)
            .Handle(CriarCommandValido(), CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Médico não encontrado.", response.Errors);
    }

    private static UpdateMedicoCommand CriarCommandValido()
    {
        return new UpdateMedicoCommand { Id = 1, Nome = "Dr. A", CRM = "CRM123", EspecialidadeId = 1, Telefone = "67999999999", Email = "a@clinica.com", Situacao = "A" };
    }
}
