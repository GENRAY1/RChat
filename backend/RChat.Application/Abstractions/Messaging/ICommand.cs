using MediatR;

namespace RChat.Application.Abstractions.Messaging;

public interface ICommand : IRequest
{
}

public interface ICommand<TReponse> : IRequest<TReponse>
{
}
