using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Client.Endpoints;

public class DokumentiEndpoints(
	Func<HttpClient> client,
	Action<HttpResponseMessage> handleStatusCode
)
{
	public async Task<DokumentDto> GetAsync(DokumentGetRequest request)
	{
		var response = await client()
			.GetAsJsonAsync($"dokumenti/{request.VrDok}/{request.BrDok}", request);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<DokumentDto>())!;
	}

	public async Task<List<DokumentDto>> GetMultipleAsync(DokumentGetMultipleRequest request)
	{
		var response = await client().GetAsJsonAsync("dokumenti", request);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<List<DokumentDto>>())!;
	}

	public async Task<DokumentDto> CreateAsync(DokumentCreateRequest request)
	{
		var response = await client().PostAsJsonAsync("dokumenti", request);
		handleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<DokumentDto>())!;
	}

	public async Task UpdateDokOut(DokumentSetDokOutRequest request)
	{
		var response = await client()
			.PutAsJsonAsync($"dokumenti/{request.VrDok}/{request.BrDok}/dok-out", request);
		handleStatusCode(response);
	}

	public async Task SetDokumenFlag(DokumentSetFlagRequest request)
	{
		handleStatusCode(
			await client()
				.PutAsync($"dokumenti/{request.VrDok}/{request.BrDok}/flag/{request.Flag}", null)
		);
	}
}
