using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TDOffice_v2
{
    public static class TDBrain_v3
    {
        #if DEBUG
        private static readonly string _apiBaseUrl = "http://localhost:7207";
        #else
        private static readonly string _apiBaseUrl = "http://4monitor:7207";
        #endif
        private static readonly HttpClient _client = new HttpClient() {
            Timeout = TimeSpan.FromSeconds(10)
        };

        public static async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            return await _client.GetAsync(_apiBaseUrl + endpoint);
        }
        public static async Task<HttpResponseMessage> PostAsync(string endpoint)
        {
            return await _client.PostAsync(_apiBaseUrl + endpoint, null);
        }
        public static Task<HttpResponseMessage> PostAsync(string endpoint, Dictionary<string, string> parameters)
        {
            return Task.Run<HttpResponseMessage>(async () =>
            {
                var content = new FormUrlEncodedContent(parameters);
                return await _client.PostAsync(_apiBaseUrl + endpoint, content);
            });
        }
    }
}
