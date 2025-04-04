namespace RChat.Application.Exceptions;

public class ChatDeletedException()
    : ValidationException("The chat has been deleted and the operation cannot be completed");