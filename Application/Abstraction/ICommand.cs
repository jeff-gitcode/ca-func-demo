using MediatR;

namespace Application.Abstraction;

public interface ICommand<TResponse> : IRequest<TResponse> { }

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse> { }
