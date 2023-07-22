using System.Net.Http;
using System.Threading.Tasks;

namespace TDOffice_v2.Core.Http
{
    public static class Extensions
    {
        public static Task<HttpResponseMessage> GetAsJsonAsync<TRequest>(this HttpClient httpClient, string endpoint, TRequest request)
        {
            string finalEndpoint = endpoint + "?";
            foreach (var propertyInfo in request.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(request);
                if (value == null)
                    continue;

                finalEndpoint += $"{propertyInfo.Name}={value}";
            }

            return httpClient.GetAsync(finalEndpoint);
        }
    }
}
