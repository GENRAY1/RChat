namespace RChat.Web.Middlewares.ExceptionMiddleware;

public record ExceptionResponse(int StatusCode, string Message);