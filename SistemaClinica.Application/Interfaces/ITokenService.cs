using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
