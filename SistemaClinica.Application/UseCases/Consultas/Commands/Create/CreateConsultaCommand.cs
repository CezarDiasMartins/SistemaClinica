using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Consultas.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;

namespace SistemaClinica.Application.UseCases.Consultas.Commands.Create;

public class CreateConsultaCommand : ConsultaCommandBase, IRequest<GenericDataResponse<ConsultaResponse>>;

public class CreateConsultaCommandHandler(
    IRepository<Consulta> consultaRepository,
    IRepository<Paciente> pacienteRepository,
    IRepository<Medico> medicoRepository,
    IRepository<Especialidade> especialidadeRepository,
    IMapper _mapper)
    : IRequestHandler<CreateConsultaCommand, GenericDataResponse<ConsultaResponse>>
{
    public async Task<GenericDataResponse<ConsultaResponse>> Handle(CreateConsultaCommand request, CancellationToken cancellationToken)
    {
        if (request.DataConsulta <= DateTime.Now)
            return new GenericDataResponse<ConsultaResponse> { Errors = ["A consulta não pode ser marcada em data passada."] };

        var paciente = await pacienteRepository.GetByIdAsync(request.PacienteId, cancellationToken);
        if (paciente is null)
            return new GenericDataResponse<ConsultaResponse> { Errors = ["Paciente não encontrado."] };

        var medico = await medicoRepository.GetByIdAsync(request.MedicoId, cancellationToken);
        if (medico is null)
            return new GenericDataResponse<ConsultaResponse> { Errors = ["Médico não encontrado."] };

        var especialidade = await especialidadeRepository.GetByIdAsync(request.EspecialidadeId, cancellationToken);
        if (especialidade is null)
            return new GenericDataResponse<ConsultaResponse> { Errors = ["Especialidade não encontrada."] };

        if (await consultaRepository.ExistsAsync(c => c.MedicoId == request.MedicoId && c.DataConsulta == request.DataConsulta, cancellationToken))
            return new GenericDataResponse<ConsultaResponse> { Errors = ["Já existe uma consulta para este médico no mesmo horário."] };

        var consulta = new Consulta
        {
            PacienteId = request.PacienteId,
            MedicoId = request.MedicoId,
            EspecialidadeId = request.EspecialidadeId,
            DataConsulta = request.DataConsulta,
            Observacao = request.Observacao.Trim(),
            SituacaoConsulta = SituacaoConsulta.Agendada
        };

        await consultaRepository.InsertAsync(consulta, cancellationToken);
        await consultaRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<ConsultaResponse> { Data = _mapper.Map<ConsultaResponse>(consulta) };
    }
}
