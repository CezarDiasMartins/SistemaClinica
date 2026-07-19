using MediatR;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Application.Response;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Libs.Extensions;

namespace SistemaClinica.Application.UseCases.Auth.Commands.Login;

public class LoginCommand : IRequest<GenericDataResponse<LoginResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class LoginCommandHandler(
    IRepository<Usuario> usuarioRepository,
    ISenhaService senhaService,
    ITokenService tokenService)
    : IRequestHandler<LoginCommand, GenericDataResponse<LoginResponse>>
{
    public async Task<GenericDataResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.NormalizarEmail();
        var usuario = await usuarioRepository.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (usuario is null || !senhaService.Verificar(request.Senha, usuario.Senha))
            return new GenericDataResponse<LoginResponse> { Errors = ["E-mail ou senha inválidos."] };

        var token = tokenService.GerarToken(usuario);

        return new GenericDataResponse<LoginResponse>
        {
            Data = new LoginResponse
            {
                Token = token,
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role.ToString()
            }
        };
    }
}
