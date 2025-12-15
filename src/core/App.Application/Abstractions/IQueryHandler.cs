using MediatR;

namespace App.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ServiceResult<TResponse>>
    where TQuery : IQuery<TResponse>
    where TResponse : class
{
}
