using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infrastructure.Framework
{
    /// <summary>
    /// Abstract class used to create API classes which are used to communicate with API
    /// </summary>
    public abstract class API
    {
        private HttpClient _httpClient { get; set; }

        /// <summary>
        /// Intializes class as API class
        /// </summary>
        /// <param name="httpClient"></param>
        public API(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));

            _httpClient = httpClient;
        }

        /// <summary>
        /// Makes GET request to the API endpoint with expected response content
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<APIResponse<TResponse>> GetAsync<TResponse>(string endpoint)
        {
            return new APIResponse<TResponse>(await _httpClient.GetAsync(endpoint));
        }
        /// <summary>
        /// Makes POST request to the API endpoint without expected response content
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<APIResponse> PostAsync(string endpoint)
        {
            return new APIResponse(await _httpClient.PostAsync(endpoint, null));
        }
        /// <summary>
        /// Makes POST request to the API endpoint without expected response content
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<APIResponse> PostAsync<TRequest>(TRequest request, string endpoint)
        {
            return new APIResponse(await _httpClient.PostAsJsonAsync(endpoint, request));
        }
        /// <summary>
        /// Makes POST request to the API endpoint with expected response content
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<APIResponse<TResponse>> PostAsync<TRequest, TResponse>(TRequest request, string endpoint)
        {
            return new APIResponse<TResponse>(await _httpClient.PostAsJsonAsync(endpoint, request));
        }
    }
}
