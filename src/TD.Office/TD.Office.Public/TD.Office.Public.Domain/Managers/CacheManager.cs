using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Domain.Managers;

public class CacheManager(IDistributedCache distributedCache) : ICacheManager
{
	public async Task<T> GetDataAsync<T>(string key)
	{
		var data = await distributedCache.GetStringAsync(key);
		if (!string.IsNullOrEmpty(data))
			return JsonConvert.DeserializeObject<T>(data);

		return default(T);
	}

	public async Task<T> GetOrUpdateDataAsync<T>(
		string key,
		Func<T> getData,
		TimeSpan absoluteExpirationInterval
	)
	{
		var dataStringTask = distributedCache.GetStringAsync(key);

		var dataString = await dataStringTask;

		var data = string.IsNullOrWhiteSpace(dataString)
			? getData()
			: JsonConvert.DeserializeObject<T>(dataString);

		_ = Task.Run(async () =>
		{
			// Should be moved to do on separate thread, however at the moment we have a problem of disposing calling method objects
			await distributedCache.SetStringAsync(
				key,
				JsonConvert.SerializeObject(data),
				new DistributedCacheEntryOptions()
				{
					AbsoluteExpirationRelativeToNow = absoluteExpirationInterval
				}
			);
		});

		return data;
	}

	public async Task RemoveDataAsync(string key) => await distributedCache.RemoveAsync(key);

	public async Task SetDataAsync<T>(
		string key,
		Func<T> getData,
		TimeSpan absoluteExpirationInterval
	) =>
		await distributedCache.SetStringAsync(
			key,
			JsonConvert.SerializeObject(getData()),
			new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = absoluteExpirationInterval
			}
		);
}
