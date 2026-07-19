using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Application.UseCases.Recepcionistas.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Domain.Enums;
using SistemaClinica.Libs.Extensions;

namespace SistemaClinica.Application.UseCases.Recepcionistas.Commands.Create;

public class CreateRecepcionistaCommand : RecepcionistaCommandBase, IRequest<GenericDataResponse<RecepcionistaResponse>>
{
    public string Senha { get; set; } = string.Empty;
}

public class CreateRecepcionistaCommandHandler(
    IRepository<Recepcionista> recepcionistaRepository,
    ISenhaService senhaService,
    IMapper _mapper)
    : IRequestHandler<CreateRecepcionistaCommand, GenericDataResponse<RecepcionistaResponse>>
{
    public async Task<GenericDataResponse<RecepcionistaResponse>> Handle(CreateRecepcionistaCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.NormalizarEmail();

        if (await recepcionistaRepository.ExistsAsync(recepcionista => recepcionista.Email == email, cancellationToken))
            return new GenericDataResponse<RecepcionistaResponse> { Errors = ["Já existe um recepcionista cadastrado com este e-mail."] };

        var recepcionista = new Recepcionista
        {
            Nome = request.Nome.Trim(),
            Email = email,
            Senha = senhaService.Criptografar(request.Senha),
            Role = RoleUsuario.Recepcionista,
            Situacao = SituacaoGeral.Ativo
        };

        await recepcionistaRepository.InsertAsync(recepcionista, cancellationToken);
        await recepcionistaRepository.SalveAsync(cancellationToken);

        return new GenericDataResponse<RecepcionistaResponse> { Data = _mapper.Map<RecepcionistaResponse>(recepcionista) };
    }
}
