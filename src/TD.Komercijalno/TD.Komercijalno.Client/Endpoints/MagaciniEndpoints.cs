using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Requests.Magacini;

namespace TD.Komercijalno.Client.Endpoints;

public class MagaciniEndpoints(
	Func<HttpClient> client,
	Action<HttpResponseMessage> handleStatusCode
)
{
	public async Task<List<MagacinDto>> GetMultipleAsync(MagaciniGetMultipleRequest request)
	{
		var response = await client().GetAsJsonAsync("magacini", request);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<List<MagacinDto>>())!;
	}
}
