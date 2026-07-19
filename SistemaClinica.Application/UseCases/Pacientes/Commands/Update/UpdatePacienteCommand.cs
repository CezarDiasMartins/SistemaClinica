using MediatR;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Pacientes.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;
using SistemaClinica.Libs.Extensions;
using System.Text.Json.Serialization;

namespace SistemaClinica.Application.UseCases.Pacientes.Commands.Update;

public class UpdatePacienteCommand : PacienteCommandBase, IRequest<GenericDataResponse<PacienteResponse>>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? Situacao { get; set; }
}

public class UpdatePacienteCommandHandler(IRepository<Paciente> pacienteRepository, IMapper _mapper)
    : IRequestHandler<UpdatePacienteCommand, GenericDataResponse<PacienteResponse>>
{
    public async Task<GenericDataResponse<PacienteResponse>> Handle(UpdatePacienteCommand request, CancellationToken cancellationToken)
    {
        var paciente = await pacienteRepository.GetByIdAsync(request.Id, cancellationToken);

        if (paciente is null)
            return new GenericDataResponse<PacienteResponse> { Errors = ["Paciente não encontrado."] };

        var cpf = request.CPF.SoNumeros();

        if (await pacienteRepository.ExistsAsync(p => p.CPF == cpf && p.Id != request.Id, cancellationToken))
            return new GenericDataResponse<PacienteResponse> { Errors = ["Já existe outro paciente cadastrado com este CPF."] };

        paciente.Nome = request.Nome.Trim();
        paciente.CPF = cpf;
        paciente.DataNascimento = request.DataNascimento;
        paciente.Telefone = request.Telefone.SoNumeros();
        paciente.Email = request.Email.NormalizarEmail();
        paciente.Sexo = request.Sexo;
        if (!SituacaoGeralHelper.TryParse(request.Situacao, out var situacao))
            return new GenericDataResponse<PacienteResponse> { Errors = ["Informe uma situação válida para o paciente."] };

        paciente.Situacao = situacao;

        pacienteRepository.Update(paciente);
        await pacienteRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<PacienteResponse> { Data = _mapper.Map<PacienteResponse>(paciente) };
    }
}
