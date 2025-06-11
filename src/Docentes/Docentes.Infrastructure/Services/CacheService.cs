using System.Text.Json;
using Docentes.Application.Services;
using StackExchange.Redis;

namespace Docentes.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<T?> GetCacheValueAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var value = await db.StringGetAsync(key);
        if (value.IsNullOrEmpty)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        var db = _connectionMultiplexer.GetDatabase();
       await db.StringSetAsync(
            key,
            JsonSerializer.Serialize(value),
            expiration
        );

    }
}