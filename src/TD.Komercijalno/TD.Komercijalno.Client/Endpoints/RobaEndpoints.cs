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

	public async Task<List<RobaDto>> GetMultipleAsync(RobaGetMultipleRequest request)
	{
		var response = await client().GetAsJsonAsync("roba", request);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<List<RobaDto>>())!;
	}
}
