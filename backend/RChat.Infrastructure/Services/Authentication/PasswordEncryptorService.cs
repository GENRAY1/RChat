using RChat.Application.Abstractions.Services;
using RChat.Application.Abstractions.Services.Authentication;

namespace RChat.Infrastructure.Services.Authentication;

public class PasswordEncryptorService : IPasswordEncryptorService
{
    public string Generate(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
    
    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}