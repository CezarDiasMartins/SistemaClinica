using MediatR;
using SistemaClinica.Application.Helpers;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Recepcionistas.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;
using SistemaClinica.Libs.Extensions;
using System.Text.Json.Serialization;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Commands.Update;

public class UpdateRecepcionistaCommand : RecepcionistaCommandBase, IRequest<GenericDataResponse<RecepcionistaResponse>>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? Senha { get; set; }
    public string? Situacao { get; set; }
}

public class UpdateRecepcionistaCommandHandler(
    IRepository<Recepcionista> recepcionistaRepository,
    ISenhaService senhaService,
    IMapper _mapper)
    : IRequestHandler<UpdateRecepcionistaCommand, GenericDataResponse<RecepcionistaResponse>>
{
    public async Task<GenericDataResponse<RecepcionistaResponse>> Handle(UpdateRecepcionistaCommand request, CancellationToken cancellationToken)
    {
        var recepcionista = await recepcionistaRepository.GetByIdAsync(request.Id, cancellationToken);

        if (recepcionista is null)
            return new GenericDataResponse<RecepcionistaResponse> { Errors = ["Recepcionista não encontrado."] };

        var email = request.Email.NormalizarEmail();

        if (await recepcionistaRepository.ExistsAsync(u => u.Email == email && u.Id != request.Id, cancellationToken))
            return new GenericDataResponse<RecepcionistaResponse> { Errors = ["Já existe outro recepcionista cadastrado com este e-mail."] };

        recepcionista.Nome = request.Nome.Trim();
        recepcionista.Email = email;
        recepcionista.Role = RoleUsuario.Recepcionista;
        if (!SituacaoGeralHelper.TryParse(request.Situacao, out var situacao))
            return new GenericDataResponse<RecepcionistaResponse> { Errors = ["Informe uma situação válida para o recepcionista."] };

        recepcionista.Situacao = situacao;

        if (!string.IsNullOrWhiteSpace(request.Senha))
            recepcionista.Senha = senhaService.Criptografar(request.Senha);

        recepcionistaRepository.Update(recepcionista);
        await recepcionistaRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<RecepcionistaResponse> { Data = _mapper.Map<RecepcionistaResponse>(recepcionista) };
    }
}
