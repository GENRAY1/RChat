namespace RChat.Application.Abstractions.Services.Authentication;

public interface IPasswordEncryptorService
{
    string Generate(string password);
    bool Verify(string password, string hash);
}