using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public static class TDBrain_v3
    {
        #if DEBUG
        private static readonly string _apiBaseUrl = "http://localhost:7206";
        #else
        private static readonly string _apiBaseUrl = "http://192.168.0.11:7207";
        #endif
        private static readonly HttpClient _client = new HttpClient() {
            Timeout = TimeSpan.FromMinutes(5)
        };

        public static async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            return await _client.GetAsync(_apiBaseUrl + endpoint);
        }
        public static async Task<HttpResponseMessage> GetAsync(string endpoint, IDictionary<string, string> parameters)
        {
            string a = _apiBaseUrl + endpoint + "?" + string.Join("&", parameters.Select(x => x.Key + "=" + x.Value));
            return await _client.GetAsync(a);
        }
        public static async Task<HttpResponseMessage> PostAsync(string endpoint)
        {
            return await _client.PostAsync(_apiBaseUrl + endpoint, null);
        }
        public static Task<HttpResponseMessage> PostAsync(string endpoint, Dictionary<string, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters);
            return _client.PostAsync(_apiBaseUrl + endpoint, content);
        }
        public static Task<HttpResponseMessage> PostAsync<TRequestBody>(string endpoint, TRequestBody requestBody)
        {
            return _client.PostAsJsonAsync(_apiBaseUrl + endpoint, requestBody);
        }
        /// <summary>
        /// Handles response from TDBrain.
        /// Handles status code 200 & 500.
        /// If response is 200, it will deserialize and return generic object
        /// If statusHandler is passed, it will first go through it.
        /// If statusHandler returns non null, default code checking will not be fired.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTask"></param>
        /// <returns></returns>
        /// <exception cref="Termodom.Data.Exceptions.APIServerException">API 500 status</exception>
        /// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException">Unhandled status</exception>
        public static Task<T> HandleTDBrainResponse<T>(this Task<HttpResponseMessage> responseTask)
        {
            return HandleTDBrainResponse<T>(responseTask, null);
        }
        /// <summary>
        /// Handles response from TDBrain.
        /// Handles status code 200 & 500.
        /// If response is 200, it will deserialize and return generic object
        /// If statusHandler is passed, it will first go through it.
        /// If statusHandler returns non null, default code checking will not be fired.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTask"></param>
        /// <param name="statusHandler"></param>
        /// <returns></returns>
        /// <exception cref="Termodom.Data.Exceptions.APIServerException">API 500 status</exception>
        /// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException">Unhandled status</exception>
        public static async Task<T> HandleTDBrainResponse<T>(this Task<HttpResponseMessage> responseTask, Func<HttpResponseMessage, T> statusHandler)
        {
            var response = await responseTask;

            if (statusHandler != null)
            {
                T responseFromStatusHandler = statusHandler(response);
                if (responseFromStatusHandler != null)
                    return responseFromStatusHandler;
            }

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
