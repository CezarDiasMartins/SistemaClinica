using System.Linq.Expressions;
using Moq;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.UseCases.Consultas.Commands.Create;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Tests.UseCases.Consultas.Commands.Create;

public class CreateConsultaCommandHandlerTest
{
    [Fact]
    public async Task Deve_Criar_Consulta_Quando_Dados_Forem_Validos()
    {
        var consultaRepository = new Mock<IRepository<Consulta>>();
        var pacienteRepository = new Mock<IRepository<Paciente>>();
        var medicoRepository = new Mock<IRepository<Medico>>();
        var especialidadeRepository = new Mock<IRepository<Especialidade>>();

        pacienteRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Paciente { Id = 1, Nome = "Maria" });
        medicoRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Medico { Id = 1, Nome = "Dr. Carlos" });
        especialidadeRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Especialidade { Id = 1, Descricao = "Clínica Geral" });
        consultaRepository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var handler = new CreateConsultaCommandHandler(consultaRepository.Object, pacienteRepository.Object, medicoRepository.Object, especialidadeRepository.Object, MapperHelper.Mapper);
        var response = await handler.Handle(CriarCommandValido(), CancellationToken.None);

        Assert.True(response.Success);
        consultaRepository.Verify(x => x.InsertAsync(It.IsAny<Consulta>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Normalizar_Data_Utc_Para_Local_Sem_Fuso_Ao_Criar_Consulta()
    {
        var consultaRepository = new Mock<IRepository<Consulta>>();
        var pacienteRepository = new Mock<IRepository<Paciente>>();
        var medicoRepository = new Mock<IRepository<Medico>>();
        var especialidadeRepository = new Mock<IRepository<Especialidade>>();
        Consulta? consultaInserida = null;

        pacienteRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Paciente { Id = 1, Nome = "Maria" });
        medicoRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Medico { Id = 1, Nome = "Dr. Carlos" });
        especialidadeRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Especialidade { Id = 1, Descricao = "Clínica Geral" });
        consultaRepository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Consulta, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        consultaRepository.Setup(x => x.InsertAsync(It.IsAny<Consulta>(), It.IsAny<CancellationToken>()))
            .Callback<Consulta, CancellationToken>((consulta, _) => consultaInserida = consulta)
            .Returns(Task.CompletedTask);

        var dataUtc = DateTime.UtcNow.AddDays(1);
        var command = CriarCommandValido();
        command.DataConsulta = dataUtc;

        var handler = new CreateConsultaCommandHandler(consultaRepository.Object, pacienteRepository.Object, medicoRepository.Object, especialidadeRepository.Object, MapperHelper.Mapper);
        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Success);
        Assert.NotNull(consultaInserida);
        Assert.Equal(DateTimeKind.Unspecified, consultaInserida!.DataConsulta.Kind);
        Assert.Equal(DateTime.SpecifyKind(dataUtc.ToLocalTime(), DateTimeKind.Unspecified), consultaInserida.DataConsulta);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Data_For_Passada()
    {
        var handler = new CreateConsultaCommandHandler(
            new Mock<IRepository<Consulta>>().Object,
            new Mock<IRepository<Paciente>>().Object,
            new Mock<IRepository<Medico>>().Object,
            new Mock<IRepository<Especialidade>>().Object,
            MapperHelper.Mapper);

        var command = CriarCommandValido();
        command.DataConsulta = DateTime.Now.AddDays(-1);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Success);
        Assert.Contains("A consulta não pode ser marcada em data passada.", response.Errors);
    }

    private static CreateConsultaCommand CriarCommandValido()
    {
        return new CreateConsultaCommand
        {
            PacienteId = 1,
            MedicoId = 1,
            EspecialidadeId = 1,
            DataConsulta = DateTime.Now.AddDays(1)
        };
    }
}
