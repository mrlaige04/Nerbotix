namespace RoboTasker.Application.Common.Abstractions;

public interface ITenantRequest;

public interface ITenantCommand : ICommand, ITenantRequest;
public interface ITenantCommand<TResponse> : ICommand<TResponse>, ITenantRequest;

public interface ITenantQuery<TResponse> : IQuery<TResponse>, ITenantRequest;