using MediatR;

namespace App.Application.Abstractions;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ServiceResult>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, ServiceResult<TResponse>>
    where TCommand : ICommand<TResponse>
    where TResponse : class
{
}
