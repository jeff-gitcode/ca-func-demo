using MediatR;

namespace Application.Abstraction;

public interface IQuery<TResponse> : IRequest<TResponse> { }

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IRequest<TResponse> { }
