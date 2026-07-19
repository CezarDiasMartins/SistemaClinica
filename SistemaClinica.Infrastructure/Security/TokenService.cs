using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Infrastructure.Security;

public class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    public string GerarToken(Usuario usuario)
    {
        var jwt = options.Value;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, usuario.Role.ToString())
        };

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwt.ExpirationMinutes),
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
