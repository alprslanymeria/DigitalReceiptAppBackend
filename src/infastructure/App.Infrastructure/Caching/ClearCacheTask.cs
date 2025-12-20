using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Infrastructure.Tasks;

namespace App.Infrastructure.Caching;

public class ClearCacheTask(

    IStaticCacheManager staticCacheManager

    ) : IScheduleTask
{

    // FIELDS
    protected readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    // IMPLEMENTATION OF ISCHEDULETASK
    public virtual async Task ExecuteAsync()
    {
        await _staticCacheManager.ClearAsync();
    }
}
