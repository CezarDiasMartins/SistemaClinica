using System.Text;
using Microsoft.EntityFrameworkCore;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Infrastructure.Context;

namespace SistemaClinica.API.Workers;

public class RelatorioConsultasDoDiaWorker(
    IServiceScopeFactory scopeFactory,
    IWebHostEnvironment environment,
    ILogger<RelatorioConsultasDoDiaWorker> logger)
    : BackgroundService
{
    private DateOnly? _ultimaDataGerada;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await GerarRelatorioDiarioComTratamento(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromHours(1));
        while (await timer.WaitForNextTickAsync(stoppingToken))
            await GerarRelatorioDiarioComTratamento(stoppingToken);
    }

    private async Task GerarRelatorioDiarioComTratamento(CancellationToken cancellationToken)
    {
        try
        {
            await GerarRelatorioDiario(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Não foi possível gerar o relatório diário de consultas.");
        }
    }

    private async Task GerarRelatorioDiario(CancellationToken cancellationToken)
    {
        var hoje = DateOnly.FromDateTime(DateTime.Now);
        if (_ultimaDataGerada == hoje)
            return;

        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SistemaClinicaDbContext>();

        var inicio = hoje.ToDateTime(TimeOnly.MinValue);
        var fim = hoje.ToDateTime(TimeOnly.MaxValue);

        var consultas = await context.Consultas
            .AsNoTracking()
            .Include(x => x.Paciente)
            .Include(x => x.Medico)
            .Include(x => x.Especialidade)
            .Where(x => x.DataConsulta >= inicio && x.DataConsulta <= fim)
            .OrderBy(x => x.DataConsulta)
            .ToListAsync(cancellationToken);

        var pasta = Path.Combine(environment.ContentRootPath, "Relatorios");
        Directory.CreateDirectory(pasta);

        var caminho = Path.Combine(pasta, $"consultas-{hoje:yyyy-MM-dd}.csv");
        await File.WriteAllTextAsync(caminho, MontarCsv(consultas), Encoding.UTF8, cancellationToken);

        _ultimaDataGerada = hoje;
        logger.LogInformation("Relatório diário de consultas gerado em {Caminho}.", caminho);
    }

    private static string MontarCsv(IEnumerable<Consulta> consultas)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Horario;Paciente;Medico;Especialidade;Situacao;Observacao");

        foreach (var consulta in consultas)
            csv.AppendLine(string.Join(';', [
                consulta.DataConsulta.ToString("HH:mm"),
                Escapar(consulta.Paciente?.Nome),
                Escapar(consulta.Medico?.Nome),
                Escapar(consulta.Especialidade?.Descricao),
                consulta.SituacaoConsulta.ToString(),
                Escapar(consulta.Observacao)
            ]));

        return csv.ToString();
    }

    private static string Escapar(string? valor)
    {
        return (valor ?? string.Empty).Replace(";", ",").ReplaceLineEndings(" ");
    }
}
