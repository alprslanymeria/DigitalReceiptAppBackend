using MediatR;

namespace App.Application.Abstractions;

public interface ICommand : IRequest<ServiceResult>
{
}

public interface ICommand<TResponse> : IRequest<ServiceResult<TResponse>> where TResponse : class
{
}
