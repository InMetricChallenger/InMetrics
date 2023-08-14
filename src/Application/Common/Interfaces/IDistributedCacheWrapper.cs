using Microsoft.Extensions.Caching.Distributed;

namespace Application.Common.Interfaces;

public interface IDistributedCacheWrapper
{
    Task<string> GetStringAsync(string key, CancellationToken cancellationToken = default);
    Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
