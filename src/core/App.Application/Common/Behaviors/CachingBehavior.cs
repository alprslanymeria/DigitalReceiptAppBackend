using App.Application.Contracts.Infrastructure.Caching;
using MediatR;

namespace App.Application.Common.Behaviors;

public class CachingBehavior<TRequest, TResponse>(IStaticCacheManager cache, ICacheKeyFactory keyFactory) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery
{
    public async Task<TResponse> Handle(

        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken

        )
    {
        var cacheKey = request.GetCacheKey(keyFactory);

        return await cache.GetAsync(
            cacheKey,
            () => next(cancellationToken)
        );
    }
}
