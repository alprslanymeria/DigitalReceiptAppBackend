using App.Application.Contracts.Infrastructure.Caching;

namespace App.Caching.CacheKey;

public class CacheKey(string key) : ICacheKey
{

    // IMPLEMENTATION OF ICacheKey
    public string Key { get; set; } = key;
    public int CacheTime { get; set; }
}
