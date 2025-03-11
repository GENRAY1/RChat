namespace RChat.Application.Exceptions;

public class ApiException(int statusCode, string message)
    : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    
    public string ErrorMessage { get; } = message;
}