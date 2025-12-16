using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.Komercijalno.Client.Endpoints;

public class StavkeEndpoints(Func<HttpClient> client, Action<HttpResponseMessage> handleStatusCode)
{
	public async Task<List<StavkaDto>> GetMultipleByRobaIdAsync(StavkeGetMultipleByRobaId request)
	{
		var response = await client().GetAsJsonAsync("stavke-by-robaid", request);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<List<StavkaDto>>())!;
	}

	public async Task<StavkaDto> CreateAsync(StavkaCreateRequest request)
	{
		var response = await client().PostAsJsonAsync("stavke", request);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<StavkaDto>())!;
	}

	public async Task DeleteAsync(StavkeDeleteRequest request)
	{
		var response = await client()
			.DeleteAsync($"stavke?VrDok={request.VrDok}&BrDok={request.BrDok}&RobaId={request.RobaId}");
		handleStatusCode(response);
	}
}
