using System.Security.Cryptography;
using SistemaClinica.Application.Interfaces;

namespace SistemaClinica.Infrastructure.Security;

public class SenhaService : ISenhaService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public string Criptografar(string senha)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(senha, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

        return $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public bool Verificar(string senha, string hash)
    {
        var partes = hash.Split('$');

        if (partes.Length != 4 || partes[0] != "PBKDF2")
            return false;

        var iterations = int.Parse(partes[1]);
        var salt = Convert.FromBase64String(partes[2]);
        var hashSalvo = Convert.FromBase64String(partes[3]);
        var hashInformado = Rfc2898DeriveBytes.Pbkdf2(senha, salt, iterations, HashAlgorithmName.SHA256, hashSalvo.Length);

        return CryptographicOperations.FixedTimeEquals(hashSalvo, hashInformado);
    }
}
