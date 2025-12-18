using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using LSCore.Common.Contracts;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Requests.Roba;

namespace TD.Komercijalno.Client.Endpoints;

public class RobaEndpoints(Func<HttpClient> client, Action<HttpResponseMessage> handleStatusCode)
{
	public async Task<RobaDto> Get(LSCoreIdRequest request)
	{
		var response = await client().GetAsync($"roba/{request.Id}");
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<RobaDto>())!;
	}

	static DateTime? __getMultipleAsyncCacheTime;
	static List<RobaDto>? __getMultipleAsyncCacheData;

	public async Task<List<RobaDto>> GetMultipleAsync(RobaGetMultipleRequest request)
	{
		if (
			__getMultipleAsyncCacheData is not null
			&& __getMultipleAsyncCacheTime.HasValue
			&& __getMultipleAsyncCacheTime > DateTime.UtcNow.AddMinutes(-1)
		)
			return __getMultipleAsyncCacheData;
		var response = await client().GetAsJsonAsync("roba", request);
		handleStatusCode(response);
		var result = (await response.Content.ReadFromJsonAsync<List<RobaDto>>())!;
		__getMultipleAsyncCacheData = result;
		__getMultipleAsyncCacheTime = DateTime.UtcNow;
		return result;
	}
}
