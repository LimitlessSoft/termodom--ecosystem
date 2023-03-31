using Infrastructure.Exceptions;
using Infrastructure.Framework;
using System.Net;
using System.Net.Http.Headers;

namespace DBMigrations
{
    public class OldApi : API
    {
        public OldApi() : base(GenerateHttpClient())
        {
        }

        private static HttpClient GenerateHttpClient()
        {
            var client = new HttpClient();
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.termodom.rs"),
            };

            var tokenResponse = client.PostAsync("/api/korisnik/gettoken?username=sasar&password=12321", null).GetAwaiter().GetResult();

            if (tokenResponse.StatusCode != HttpStatusCode.OK)
                throw new APIUnhandledException(tokenResponse);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            return client;
        }

    }
}
