using System.Net.Http.Json;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;

namespace TD.Komercijalno.Client.Endpoints;

public class VrstaDokEndpoints(
	Func<HttpClient> client,
	Action<HttpResponseMessage> handleStatusCode
)
{
	public async Task<List<VrstaDokDto>> GetMultiple()
	{
		var response = await client().GetAsync("vrste-dokumenata");
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<List<VrstaDokDto>>())!;
	}
}
