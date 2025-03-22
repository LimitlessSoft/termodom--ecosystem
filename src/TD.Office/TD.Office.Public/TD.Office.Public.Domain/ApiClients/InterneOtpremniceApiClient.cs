using Microsoft.Extensions.Configuration;
using TD.Office.Public.Contracts.Interfaces.IApiClients;

namespace TD.Office.Public.Domain.ApiClients;

public class InterneOtpremniceApiClient : IInterneOtpremniceApiClient
{
	private readonly HttpClient _httpClient = new();

	public InterneOtpremniceApiClient(IConfigurationRoot configurationRoot)
	{
		_httpClient.BaseAddress = new Uri(configurationRoot["TD_OFFICE_INTERNE_OTPREMNICE_API"]!);
		// _httpClient.DefaultRequestHeaders.Add(LSCoreContractsConstants.ApiKeyCustomHeader, configurationRoot["TD_OFFICE_INTERNE_OTPREMNICE_API_KEY"]!);
	}
}
