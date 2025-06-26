using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using LSCore.Exceptions;
using TD.Office.PregledIUplataPazara.Contracts.Requests;
using TD.Office.PregledIUplataPazara.Contracts.Responses;

namespace TD.Office.PregledIUplataPazara.Client;

public class TDOfficePregledIUplataPazaraClient(
	LSCoreApiClientRestConfiguration<TDOfficePregledIUplataPazaraClient> configuration
) : LSCoreApiClient(configuration)
{
	public async Task<PregledIUplataPazaraResponse> GetMultiple(
		GetPregledIUplataPazaraRequest request
	)
	{
		var response = await _httpClient.GetAsJsonAsync("/pregled-i-uplata-pazara", request);
		if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		HandleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<PregledIUplataPazaraResponse>())!;
	}
}
