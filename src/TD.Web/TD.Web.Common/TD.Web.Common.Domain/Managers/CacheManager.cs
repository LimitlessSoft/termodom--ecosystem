using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Domain.Managers;

public class CacheManager(
	IDistributedCache distributedCache,
	IConfigurationRoot configurationRoot,
	ISettingRepository settingRepository
) : ICacheManager
{
	public async Task<T> GetDataAsync<T>(
		string key,
		Func<T> getData,
		TimeSpan absoluteExpirationInterval
	)
	{
		var cacheHash = settingRepository.GetValueAsync<string>(SettingKey.CACHE_HASH);
		key += "-" + cacheHash;
		var dataStringTask = distributedCache.GetStringAsync(key);
		var dataString = await dataStringTask;

		var data =
			string.IsNullOrWhiteSpace(dataString) || configurationRoot["DEPLOY_ENV"] == "automation"
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
}
