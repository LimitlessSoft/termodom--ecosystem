using System.Net;
using System.Net.Http.Json;
using LSCore.Exceptions;
using TD.Office.Public.Contracts.Dtos.KomercijalnoMagacinFirma;

namespace TD.Office.Public.Client.Endpoints;

public class KomercijalnoMagacinFirmaEndpoints(
	Func<HttpClient> client,
	Action<HttpResponseMessage> handleStatusCode
)
{
	public async Task<GetKomercijalnoMagacinFirmaDto> Get(int magacinId)
	{
		var response = await client().GetAsync($"/komercijalno-magacin-firma/{magacinId}");
		if (response.StatusCode == HttpStatusCode.NotFound)
			throw new LSCoreBadRequestException($"Magacin {magacinId} nije povezan sa firmom!"); // Povezati u TDOffice bazi > tabela KomercijalnoMagacinFirma
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<GetKomercijalnoMagacinFirmaDto>())!;
	}
}
