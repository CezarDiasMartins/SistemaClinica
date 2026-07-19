using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Pacientes.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;
using SistemaClinica.Libs.Extensions;

namespace SistemaClinica.Application.UseCases.Pacientes.Commands.Create;

public class CreatePacienteCommand : PacienteCommandBase, IRequest<GenericDataResponse<PacienteResponse>>;

public class CreatePacienteCommandHandler(IRepository<Paciente> pacienteRepository, IMapper _mapper)
    : IRequestHandler<CreatePacienteCommand, GenericDataResponse<PacienteResponse>>
{
    public async Task<GenericDataResponse<PacienteResponse>> Handle(CreatePacienteCommand request, CancellationToken cancellationToken)
    {
        var cpf = request.CPF.SoNumeros();

        if (await pacienteRepository.ExistsAsync(paciente => paciente.CPF == cpf, cancellationToken))
            return new GenericDataResponse<PacienteResponse> { Errors = ["Já existe um paciente cadastrado com este CPF."] };

        var paciente = new Paciente
        {
            Nome = request.Nome.Trim(),
            CPF = cpf,
            DataNascimento = request.DataNascimento,
            Telefone = request.Telefone.SoNumeros(),
            Email = request.Email.NormalizarEmail(),
            Senha = string.Empty,
            Sexo = request.Sexo,
            Role = RoleUsuario.Paciente,
            Situacao = SituacaoGeral.Ativo
        };

        await pacienteRepository.InsertAsync(paciente, cancellationToken);
        await pacienteRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<PacienteResponse> { Data = _mapper.Map<PacienteResponse>(paciente) };
    }
}
