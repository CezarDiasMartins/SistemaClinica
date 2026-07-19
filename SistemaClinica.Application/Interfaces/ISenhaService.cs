namespace SistemaClinica.Application.Interfaces;

public interface ISenhaService
{
    string Criptografar(string senha);
    bool Verificar(string senha, string hash);
}
