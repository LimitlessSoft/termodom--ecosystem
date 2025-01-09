using System.Net;
using System.Net.Http.Json;
using LSCore.Contracts.Exceptions;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Requests.Parametri;

namespace TD.Komercijalno.Client;

public class TDKomercijalnoClient
{
    private readonly HttpClient _httpClient = new();
    public ParametriEndpoints Parametri { get; init; }

    public TDKomercijalnoClient(int year, TDKomercijalnoEnvironment environment)
    {
        _httpClient.BaseAddress = new Uri(Constants.KomercijalnoApiUrlFormat(year, environment));
        Parametri = new ParametriEndpoints(() => _httpClient);
    }
    
    public static void HandleStatusCode(HttpResponseMessage? response)
    {
        if (response == null)
            throw new LSCoreBadRequestException("Response is null.");

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return;
            case HttpStatusCode.BadRequest:
                throw new LSCoreBadRequestException("Microservice API returned bad request.");
            case HttpStatusCode.Unauthorized:
                throw new LSCoreUnauthenticatedException();
            case HttpStatusCode.Forbidden:
                throw new LSCoreForbiddenException();
            case HttpStatusCode.NotFound:
                throw new LSCoreNotFoundException();
            default:
                throw new LSCoreBadRequestException("Microservice API returned unhandled exception.");
        }
    }
}