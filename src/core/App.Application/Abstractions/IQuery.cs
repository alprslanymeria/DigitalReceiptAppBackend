using MediatR;

namespace App.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<ServiceResult<TResponse>> where TResponse : class
{
}
