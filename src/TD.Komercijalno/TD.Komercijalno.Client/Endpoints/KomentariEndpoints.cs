using System.Net.Http.Json;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Client.Endpoints;

public class KomentariEndpoints(
	Func<HttpClient> client,
	Action<HttpResponseMessage> handleStatusCode
)
{
	public async Task<KomentarDto> CreateAsync(CreateKomentarRequest request)
	{
		var response = await client().PostAsJsonAsync("komentari", request);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<KomentarDto>())!;
	}

	public async Task Flush(FlushCommentsRequest request)
	{
		var response = await client().PostAsJsonAsync($"komentari-flush", request);
		handleStatusCode(response);
	}
}
