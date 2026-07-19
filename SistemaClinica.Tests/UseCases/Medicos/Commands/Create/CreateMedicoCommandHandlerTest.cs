using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Medicos.Commands.Create;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Medicos.Commands.Create;

public class CreateMedicoCommandHandlerTest
{
    [Fact]
    public async Task Deve_Criar_Medico_Quando_Dados_Forem_Validos()
    {
        var medicoRepository = new Mock<IRepository<Medico>>();
        var especialidadeRepository = new Mock<IRepository<Especialidade>>();
        medicoRepository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Medico, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        especialidadeRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Especialidade { Id = 1, Descricao = "Ortopedia" });

        var response = await new CreateMedicoCommandHandler(medicoRepository.Object, especialidadeRepository.Object, MapperHelper.Mapper)
            .Handle(CriarCommandValido(), CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal(1, response.Data!.EspecialidadeId);
        medicoRepository.Verify(x => x.InsertAsync(It.IsAny<Medico>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_CRM_Ja_Existir()
    {
        var medicoRepository = new Mock<IRepository<Medico>>();
        medicoRepository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Medico, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var response = await new CreateMedicoCommandHandler(medicoRepository.Object, new Mock<IRepository<Especialidade>>().Object, MapperHelper.Mapper)
            .Handle(CriarCommandValido(), CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Já existe um médico cadastrado com este CRM.", response.Errors);
    }

    private static CreateMedicoCommand CriarCommandValido()
    {
        return new CreateMedicoCommand { Nome = "Dr. A", CRM = "crm123", EspecialidadeId = 1, Telefone = "67999999999", Email = "a@clinica.com" };
    }
}
