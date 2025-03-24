using LSCore.ApiClient.Rest;
using TD.Komercijalno.Client.Endpoints;

namespace TD.Komercijalno.Client;

public class TDKomercijalnoClient : LSCoreApiClient
{
    public ParametriEndpoints Parametri { get; private set; }
    public MagaciniEndpoints Magacini { get; private set; }
    public RobaEndpoints Roba { get; private set; }
    public DokumentiEndpoints Dokumenti { get; private set; }
    public StavkeEndpoints Stavke { get; private set; }

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
        Roba = new RobaEndpoints(() => _httpClient, HandleStatusCode);
        Dokumenti = new DokumentiEndpoints(() => _httpClient, HandleStatusCode);
        Stavke = new StavkeEndpoints(() => _httpClient, HandleStatusCode);
    }
}
