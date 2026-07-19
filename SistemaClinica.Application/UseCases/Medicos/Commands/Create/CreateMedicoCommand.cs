using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Medicos.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;
using SistemaClinica.Libs.Extensions;

namespace SistemaClinica.Application.UseCases.Medicos.Commands.Create;

public class CreateMedicoCommand : MedicoCommandBase, IRequest<GenericDataResponse<MedicoResponse>>;

public class CreateMedicoCommandHandler(
    IRepository<Medico> medicoRepository,
    IRepository<Especialidade> especialidadeRepository,
    IMapper _mapper)
    : IRequestHandler<CreateMedicoCommand, GenericDataResponse<MedicoResponse>>
{
    public async Task<GenericDataResponse<MedicoResponse>> Handle(CreateMedicoCommand request, CancellationToken cancellationToken)
    {
        var crm = request.CRM.Trim().ToUpperInvariant();

        if (await medicoRepository.ExistsAsync(medico => medico.CRM == crm, cancellationToken))
            return new GenericDataResponse<MedicoResponse> { Errors = ["Já existe um médico cadastrado com este CRM."] };

        if (await especialidadeRepository.GetByIdAsync(request.EspecialidadeId, cancellationToken) is null)
            return new GenericDataResponse<MedicoResponse> { Errors = ["Especialidade não encontrada."] };

        var medico = new Medico
        {
            Nome = request.Nome.Trim(),
            CRM = crm,
            EspecialidadeId = request.EspecialidadeId,
            Telefone = request.Telefone.SoNumeros(),
            Email = request.Email.NormalizarEmail(),
            Senha = string.Empty,
            Role = RoleUsuario.Medico,
            Situacao = SituacaoGeral.Ativo
        };

        await medicoRepository.InsertAsync(medico, cancellationToken);
        await medicoRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<MedicoResponse> { Data = _mapper.Map<MedicoResponse>(medico) };
    }
}
