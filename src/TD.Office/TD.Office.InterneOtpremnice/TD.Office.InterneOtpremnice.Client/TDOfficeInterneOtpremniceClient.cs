using System.Net;
using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using LSCore.Common.Contracts;
using LSCore.Exceptions;
using LSCore.SortAndPage.Contracts;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Client;

public class TDOfficeInterneOtpremniceClient(
	LSCoreApiClientRestConfiguration<TDOfficeInterneOtpremniceClient> configuration
) : LSCoreApiClient(configuration)
{
	public async Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(
		GetMultipleRequest request
	)
	{
		var response = await _httpClient.GetAsJsonAsync("/interne-otpremnice", request);
		if (response.StatusCode == HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		HandleStatusCode(response);
		return (
			await response.Content.ReadFromJsonAsync<
				LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>
			>()
		)!;
	}

	public async Task<InternaOtpremnicaDto> CreateAsync(InterneOtpremniceCreateRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("/interne-otpremnice", request);
		if (response.StatusCode == HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		HandleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<InternaOtpremnicaDto>())!;
	}

	public async Task<InternaOtpremnicaDetailsDto> GetAsync(LSCoreIdRequest request)
	{
		var response = await _httpClient.GetAsync($"/interne-otpremnice/{request.Id}");
		if (response.StatusCode == HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		HandleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<InternaOtpremnicaDetailsDto>())!;
	}

	public async Task<InternaOtpremnicaItemDto> SaveItemAsync(
		InterneOtpremniceItemCreateRequest request
	)
	{
		var response = await _httpClient.PutAsJsonAsync(
			$"/interne-otpremnice/{request.InternaOtpremnicaId}/items",
			request
		);
		if (response.StatusCode == HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		HandleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<InternaOtpremnicaItemDto>())!;
	}

	public async Task DeleteItemAsync(InterneOtpremniceItemDeleteRequest request)
	{
		var response = await _httpClient.DeleteAsync(
			$"/interne-otpremnice/{request.Id}/items/{request.ItemId}"
		);
		if (response.StatusCode == HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		HandleStatusCode(response);
	}

	public async Task ChangeStateAsync(LSCoreIdRequest request, InternaOtpremnicaStatus state)
	{
		var response = await _httpClient.PostAsync(
			$"/interne-otpremnice/{request.Id}/state/{state}",
			null
		);
		if (response.StatusCode == HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		HandleStatusCode(response);
	}

	public async Task<InternaOtpremnicaDetailsDto> ForwardToKomercijalno(LSCoreIdRequest request)
	{
		var response = await _httpClient.PostAsync(
			$"/interne-otpremnice/{request.Id}/forward-to-komercijalno",
			null
		);
		if (response.StatusCode == HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		HandleStatusCode(response);
		return (await response.Content.ReadFromJsonAsync<InternaOtpremnicaDetailsDto>())!;
	}
}
