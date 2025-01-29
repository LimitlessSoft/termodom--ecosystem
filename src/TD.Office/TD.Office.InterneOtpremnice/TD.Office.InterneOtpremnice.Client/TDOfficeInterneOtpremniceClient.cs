using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using LSCore.Contracts.Responses;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
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
        HandleStatusCode(response);
        var a = await response.Content.ReadAsStringAsync();
        return (
            await response.Content.ReadFromJsonAsync<
                LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>
            >()
        )!;
    }
}
