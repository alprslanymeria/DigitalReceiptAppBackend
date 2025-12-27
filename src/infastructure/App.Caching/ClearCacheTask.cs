using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Infrastructure.Tasks;

namespace App.Caching;

public class ClearCacheTask(

    IStaticCacheManager staticCacheManager

    ) : IScheduleTask
{

    // FIELDS
    protected readonly IStaticCacheManager StaticCacheManager = staticCacheManager;

    // IMPLEMENTATION OF IScheduleTask
    public virtual async Task ExecuteAsync()
    {
        await StaticCacheManager.ClearAsync();
    }
}
