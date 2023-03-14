using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Termodom.Data.Managers
{
    public static class TDBrainManager
    {
        public static string API_BASE_URL { get; set; }
        private static bool HasApiBaseUrl { get => !string.IsNullOrWhiteSpace(API_BASE_URL); }

        private static readonly string _apiBaseUrl = "http://localhost:7207";

        private static readonly HttpClient _client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        public static async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            if (!HasApiBaseUrl)
                throw new Exception("Set API_BASE_URL before calling any method!");

            return await _client.GetAsync(_apiBaseUrl + endpoint);
        }
        public static async Task<HttpResponseMessage> GetAsync(string endpoint, IDictionary<string, string> parameters)
        {
            if (!HasApiBaseUrl)
                throw new Exception("Set API_BASE_URL before calling any method!");

            string a = _apiBaseUrl + endpoint + "?" + string.Join("&", parameters.Select(x => x.Key + "=" + x.Value));
            return await _client.GetAsync(a);
        }
        public static Task<HttpResponseMessage> PutAsync(string endpoint, Dictionary<string, string> parameters)
        {
            if (!HasApiBaseUrl)
                throw new Exception("Set API_BASE_URL before calling any method!");

            return Task.Run<HttpResponseMessage>(async () =>
            {
                var content = new StringContent(JsonConvert.SerializeObject(parameters), System.Text.Encoding.UTF8, "application/json");
                return await _client.PutAsync(_apiBaseUrl + endpoint, content);
            });
        }
        public static async Task<HttpResponseMessage> PostAsync(string endpoint)
        {
            if (!HasApiBaseUrl)
                throw new Exception("Set API_BASE_URL before calling any method!");

            return await _client.PostAsync(_apiBaseUrl + endpoint, null);
        }
        public static Task<HttpResponseMessage> PostAsync(string endpoint, Dictionary<string, string> parameters)
        {
            if (!HasApiBaseUrl)
                throw new Exception("Set API_BASE_URL before calling any method!");

            return Task.Run<HttpResponseMessage>(async () =>
            {
                var content = new StringContent(JsonConvert.SerializeObject(parameters), System.Text.Encoding.UTF8, "application/json");
                return await _client.PostAsync(_apiBaseUrl + endpoint, content);
            });
        }
    }
}
