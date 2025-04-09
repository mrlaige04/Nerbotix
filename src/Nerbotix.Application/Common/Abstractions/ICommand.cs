using ErrorOr;
using MediatR;

namespace Nerbotix.Application.Common.Abstractions;

public interface ICommand : IRequest<ErrorOr<Success>>;
public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>;

public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest, ErrorOr<Success>>
where TRequest : ICommand;

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, ErrorOr<TResponse>>
where TRequest : ICommand<TResponse>;