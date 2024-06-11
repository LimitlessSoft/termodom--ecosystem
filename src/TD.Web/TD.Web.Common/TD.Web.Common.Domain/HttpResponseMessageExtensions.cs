using System.Net;
using LSCore.Contracts.Exceptions;

namespace TD.Web.Common.Domain;

public static class HttpResponseMessageExtensions
{
    public static void HandleStatusCode(this HttpResponseMessage? response)
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