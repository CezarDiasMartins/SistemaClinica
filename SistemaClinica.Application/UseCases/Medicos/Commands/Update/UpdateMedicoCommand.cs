using MediatR;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Medicos.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;
using SistemaClinica.Libs.Extensions;
using System.Text.Json.Serialization;

namespace SistemaClinica.Application.UseCases.Medicos.Commands.Update;

public class UpdateMedicoCommand : MedicoCommandBase, IRequest<GenericDataResponse<MedicoResponse>>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? Situacao { get; set; }
}

public class UpdateMedicoCommandHandler(
    IRepository<Medico> medicoRepository,
    IRepository<Especialidade> especialidadeRepository,
    IMapper _mapper)
    : IRequestHandler<UpdateMedicoCommand, GenericDataResponse<MedicoResponse>>
{
    public async Task<GenericDataResponse<MedicoResponse>> Handle(UpdateMedicoCommand request, CancellationToken cancellationToken)
    {
        var medico = await medicoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (medico is null)
            return new GenericDataResponse<MedicoResponse> { Errors = ["Médico não encontrado."] };

        var crm = request.CRM.Trim().ToUpperInvariant();

        if (await medicoRepository.ExistsAsync(m => m.CRM == crm && m.Id != request.Id, cancellationToken))
            return new GenericDataResponse<MedicoResponse> { Errors = ["Já existe outro médico cadastrado com este CRM."] };

        if (await especialidadeRepository.GetByIdAsync(request.EspecialidadeId, cancellationToken) is null)
            return new GenericDataResponse<MedicoResponse> { Errors = ["Especialidade não encontrada."] };

        medico.Nome = request.Nome.Trim();
        medico.CRM = crm;
        medico.EspecialidadeId = request.EspecialidadeId;
        medico.Telefone = request.Telefone.SoNumeros();
        medico.Email = request.Email.NormalizarEmail();
        if (!SituacaoGeralHelper.TryParse(request.Situacao, out var situacao))
            return new GenericDataResponse<MedicoResponse> { Errors = ["Informe uma situação válida para o médico."] };

        medico.Situacao = situacao;

        medicoRepository.Update(medico);
        await medicoRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<MedicoResponse> { Data = _mapper.Map<MedicoResponse>(medico) };
    }
}
