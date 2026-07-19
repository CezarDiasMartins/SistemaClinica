using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Consultas.Commands.Update;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Consultas.Commands.Update;

public class UpdateConsultaCommandHandlerTest
{
    [Fact]
    public async Task Deve_Atualizar_Consulta_Quando_Dados_Forem_Validos()
    {
        var consultaRepository = new Mock<IRepository<Consulta>>();
        var pacienteRepository = new Mock<IRepository<Paciente>>();
        var medicoRepository = new Mock<IRepository<Medico>>();
        var especialidadeRepository = new Mock<IRepository<Especialidade>>();

        consultaRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Consulta, object>>[]>()))
            .ReturnsAsync(new Consulta { Id = 1 });
        pacienteRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Paciente { Id = 1 });
        medicoRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Medico { Id = 1 });
        especialidadeRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Especialidade { Id = 1 });
        consultaRepository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var handler = new UpdateConsultaCommandHandler(consultaRepository.Object, pacienteRepository.Object, medicoRepository.Object, especialidadeRepository.Object, MapperHelper.Mapper);
        var response = await handler.Handle(CriarCommandValido(), CancellationToken.None);

        Assert.True(response.Success);
        consultaRepository.Verify(x => x.Update(It.IsAny<Consulta>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Consulta_Nao_Existir()
    {
        var consultaRepository = new Mock<IRepository<Consulta>>();
        consultaRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Consulta, object>>[]>()))
            .ReturnsAsync((Consulta?)null);

        var handler = new UpdateConsultaCommandHandler(consultaRepository.Object, new Mock<IRepository<Paciente>>().Object, new Mock<IRepository<Medico>>().Object, new Mock<IRepository<Especialidade>>().Object, MapperHelper.Mapper);
        var response = await handler.Handle(CriarCommandValido(), CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("Consulta não encontrada.", response.Errors);
    }

    private static UpdateConsultaCommand CriarCommandValido()
    {
        return new UpdateConsultaCommand { Id = 1, PacienteId = 1, MedicoId = 1, EspecialidadeId = 1, DataConsulta = DateTime.Now.AddDays(1) };
    }
}
