using System.Security.Claims;
using SistemaClinica.Application.Interfaces;

namespace SistemaClinica.API.Services;

public class UsuarioLogadoService(IHttpContextAccessor httpContextAccessor) : IUsuarioLogadoService
{
    public int? UsuarioId
    {
        get
        {
            var valor = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(valor, out var id) ? id : null;
        }
    }

    public string? Nome => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
    public string? Email => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
    public string? Role => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
}
