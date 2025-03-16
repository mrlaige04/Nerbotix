namespace RoboTasker.Application.Common.Abstractions;

public interface ISuperAdminRequest;
public interface ISuperAdminCommand : ICommand, ISuperAdminRequest;
public interface ISuperAdminCommand<TResponse> : ICommand<TResponse>, ISuperAdminRequest;
public interface ISuperAdminQuery<TResponse> : IQuery<TResponse>, ISuperAdminRequest;