using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Termodom.API
{
    /// <summary>
    /// Koristi se za pravljenje requestova ka API-ju
    /// </summary>
    public static class APIRequest
    {
        public static HttpStatusCode[] RetryOnStatusCodes { get; set; } = new HttpStatusCode[1]
        {
            HttpStatusCode.Forbidden // 403
        };
        public static int MaxRetries { get; set; } = 10;
        public static TimeSpan RetryTimeout { get; set; } = TimeSpan.FromMilliseconds(500);

        private static HttpRequestMessage Clone(this HttpRequestMessage oldRequest)
        {
            HttpRequestMessage request = new HttpRequestMessage(oldRequest.Method, oldRequest.RequestUri);
            foreach(var header in oldRequest.Headers)
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (oldRequest.Content != null)
            {
                MemoryStream memory = new MemoryStream();
                oldRequest.Content.CopyToAsync(memory);
                memory.Position = 0;
                request.Content = new StreamContent(memory);

                foreach (var header in oldRequest.Content.Headers)
                    request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return request;
        }
        /// <summary>
        /// Salje request odredjenom serveru. Ukoliko server odgovori sa bilo kojim responsom koji se nalazi u
        /// APIRequest.RetryOnStatusCodes reqest ce biti ponovljen.
        /// Request ce se ponoviti APIRequest.MaxRetries puta sa pauzom od APIRequest.RetryTimeout izmedju svakog ponavljanja.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static HttpResponseMessage Send(HttpRequestMessage request, out APIRequestFailedLog log)
        {
            log = new APIRequestFailedLog();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Program.APIToken);

            for (int i = 0; i < MaxRetries; i++)
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);
                HttpResponseMessage response = client.Send(request.Clone());

                if (RetryOnStatusCodes.Contains(response.StatusCode))
                {
                    log.Add(new APIRequestFailedLogItem() { Request = request, Response = response, Time = DateTime.Now });
                    Thread.Sleep(RetryTimeout);

                    if(response.StatusCode == HttpStatusCode.Forbidden)
                        Program.RefreshTokenAsync().Wait();

                    continue;
                }

                return response;
            }

            throw new APIRequestTimeoutException();
        }
    }
}
