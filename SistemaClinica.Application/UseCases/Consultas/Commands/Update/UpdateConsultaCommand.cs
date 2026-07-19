using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Consultas.Response;
using SistemaClinica.Domain.Entities;
using System.Text.Json.Serialization;

namespace SistemaClinica.Application.UseCases.Consultas.Commands.Update;

public class UpdateConsultaCommand : ConsultaCommandBase, IRequest<GenericDataResponse<ConsultaResponse>>
{
    [JsonIgnore]
    public int Id { get; set; }
}

public class UpdateConsultaCommandHandler(
    IRepository<Consulta> consultaRepository,
    IRepository<Paciente> pacienteRepository,
    IRepository<Medico> medicoRepository,
    IRepository<Especialidade> especialidadeRepository,
    IMapper _mapper)
    : IRequestHandler<UpdateConsultaCommand, GenericDataResponse<ConsultaResponse>>
{
    public async Task<GenericDataResponse<ConsultaResponse>> Handle(UpdateConsultaCommand request, CancellationToken cancellationToken)
    {
        var consulta = await consultaRepository.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken, c => c.Paciente!, c => c.Medico!);

        if (consulta is null)
            return new GenericDataResponse<ConsultaResponse> { Errors = ["Consulta não encontrada."] };

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

        if (await consultaRepository.ExistsAsync(c => c.MedicoId == request.MedicoId && c.DataConsulta == request.DataConsulta && c.Id != request.Id, cancellationToken))
            return new GenericDataResponse<ConsultaResponse> { Errors = ["Já existe outra consulta para este médico no mesmo horário."] };

        consulta.PacienteId = request.PacienteId;
        consulta.MedicoId = request.MedicoId;
        consulta.EspecialidadeId = request.EspecialidadeId;
        consulta.DataConsulta = request.DataConsulta;
        consulta.Observacao = request.Observacao.Trim();
        consulta.SituacaoConsulta = request.SituacaoConsulta;

        consultaRepository.Update(consulta);
        await consultaRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<ConsultaResponse> { Data = _mapper.Map<ConsultaResponse>(consulta) };
    }
}
