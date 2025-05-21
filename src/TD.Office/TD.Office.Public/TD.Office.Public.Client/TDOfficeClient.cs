using LSCore.ApiClient.Rest;
using TD.Office.Public.Client.Endpoints;

namespace TD.Office.Public.Client;

public class TDOfficeClient : LSCoreApiClient
{
	public KomercijalnoMagacinFirmaEndpoints KomercijalnoMagacinFirma { get; private set; }

	public TDOfficeClient(LSCoreApiClientRestConfiguration<TDOfficeClient> configuration)
		: base(configuration)
	{
		Initialize();
	}

	private void Initialize()
	{
		KomercijalnoMagacinFirma = new KomercijalnoMagacinFirmaEndpoints(
			() => _httpClient,
			HandleStatusCode
		);
	}
}
