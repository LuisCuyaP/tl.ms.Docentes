namespace Docentes.Application.Services;

public interface ICacheService
{
    Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default);
    Task<T?> GetCacheValueAsync<T>(string key, CancellationToken cancellationToken = default);
}