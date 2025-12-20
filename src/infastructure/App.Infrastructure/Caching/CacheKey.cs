using App.Application.Contracts.Infrastructure.Caching;

namespace App.Infrastructure.Caching;

public class CacheKey(string key) : ICacheKey
{

    // IMPLEMENTATION OF ICACHEKEY
    public string Key { get; set; } = key;
    public int CacheTime { get; set; }
}
