namespace RChat.Application.Services.Authentication;

public interface IPasswordEncryptorService
{
    string Generate(string password);
    bool Verify(string password, string hash);
}