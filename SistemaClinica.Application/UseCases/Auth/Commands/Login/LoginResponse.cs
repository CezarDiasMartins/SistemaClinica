namespace SistemaClinica.Application.UseCases.Auth.Commands.Login;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
