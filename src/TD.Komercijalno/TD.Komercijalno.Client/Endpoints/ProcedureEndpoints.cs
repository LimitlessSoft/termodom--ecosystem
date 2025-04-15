using System.Net.Http.Json;
using TD.Komercijalno.Contracts.Requests.Procedure;

namespace TD.Komercijalno.Client.Endpoints;

public class ProcedureEndpoints(
	Func<HttpClient> client,
	Action<HttpResponseMessage?> handleStatusCode
)
{
	public async Task<decimal> GetProdajnaCenaNaDanAsync(
		ProceduraGetProdajnaCenaNaDanRequest request
	)
	{
		var response = await client()
			.GetAsync(
				$"/procedure/prodajna-cena-na-dan?magacinId={request.MagacinId}&datum={request.Datum:yyyy-MM-ddT00:00:00.000Z}&robaId={request.RobaId}"
			);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<decimal>())!;
	}
}
