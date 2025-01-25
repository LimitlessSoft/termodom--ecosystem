using System.Net;
using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using LSCore.Contracts.Exceptions;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Requests.Parametri;

namespace TD.Komercijalno.Client;

public class TDKomercijalnoClient : LSCoreApiClient
{
    public ParametriEndpoints Parametri { get; private set; }
    public MagaciniEndpoints Magacini { get; private set; }

    public TDKomercijalnoClient(
        LSCoreApiClientRestConfiguration<TDKomercijalnoClient> configuration
    )
        : base(configuration)
    {
        Initialize();
    }

    public TDKomercijalnoClient(
        int year,
        TDKomercijalnoEnvironment environment,
        TDKomercijalnoFirma firma
    )
        : base(
            new LSCoreApiClientRestConfiguration<TDKomercijalnoClient>
            {
                BaseUrl = Constants.KomercijalnoApiUrlFormat(year, environment, firma)
            }
        )
    {
        Initialize();
    }

    private void Initialize()
    {
        Parametri = new ParametriEndpoints(() => _httpClient, HandleStatusCode);
        Magacini = new MagaciniEndpoints(() => _httpClient, HandleStatusCode);
    }
}
