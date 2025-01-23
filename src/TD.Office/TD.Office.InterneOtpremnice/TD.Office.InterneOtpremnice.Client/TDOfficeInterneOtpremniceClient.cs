using LSCore.Contracts;

namespace TD.Office.InterneOtpremnice.Client;

public class TDOfficeInterneOtpremniceClient
{
    private readonly HttpClient _httpClient = new();
    
    public TDOfficeInterneOtpremniceClient(TDInterneOtpremniceEnvironmentConfiguration configuration)
    {
        #if DEBUG
        _httpClient.BaseAddress = new Uri("https://localhost:5262");
        _httpClient.DefaultRequestHeaders.Add(LSCoreContractsConstants.ApiKeyCustomHeader, "develop");
        #else
        _httpClient.BaseAddress = new Uri(Constants.ApiUrlFormat(configuration.Environment));
        _httpClient.DefaultRequestHeaders.Add(LSCoreContractsConstants.ApiKeyCustomHeader, configuration.ApiKey);
        #endif
    }
}