using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Common.Domain.Managers;

public class CacheManager(IDistributedCache distributedCache) : ICacheManager
{
    public async Task<T> GetDataAsync<T>(string key, Func<T> getData)
    {
        var dataStringTask = distributedCache.GetStringAsync(key);

        var dataString = await dataStringTask;

        var data = string.IsNullOrWhiteSpace(dataString)
            ? getData()
            : JsonConvert.DeserializeObject<T>(dataString);

        _ = Task.Run(async () =>
        {
            // Should be moved to do on separate thread, however at the moment we have a problem of disposing calling method objects
            await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(data));
        });

        return data;
    }
}
