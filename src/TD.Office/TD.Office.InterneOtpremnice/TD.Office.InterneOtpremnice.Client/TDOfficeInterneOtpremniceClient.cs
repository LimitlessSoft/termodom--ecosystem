using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;

namespace TD.Office.InterneOtpremnice.Client;

public class TDOfficeInterneOtpremniceClient(
    LSCoreApiClientRestConfiguration<TDOfficeInterneOtpremniceClient> configuration
) : LSCoreApiClient(configuration)
{
    public async Task<List<InternaOtpremnicaDto>> GetMultipleAsync()
    {
        var response = await _httpClient.GetAsync("/interne-otpremnice");
        HandleStatusCode(response);
        var a = await response.Content.ReadAsStringAsync();
        return (await response.Content.ReadFromJsonAsync<List<InternaOtpremnicaDto>>())!;
    }
}
