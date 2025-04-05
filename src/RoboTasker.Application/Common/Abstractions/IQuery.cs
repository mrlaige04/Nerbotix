using MediatR;
using ErrorOr;

namespace RoboTasker.Application.Common.Abstractions;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ErrorOr<TResponse>>
where TQuery : IQuery<TResponse>;