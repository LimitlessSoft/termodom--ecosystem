using Newtonsoft.Json;
using System.Net.Http.Json;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;

namespace TD.Core.Domain.Managers
{
    public class ApiManager : IApiManager
    {
        public HttpClient HttpClient { get; set; }

        public ApiManager()
        {
            HttpClient = new HttpClient();

            //HttpClient.BaseAddress = new Uri("https://api.termodom.rs");
            //var response = PostAsync<string>(WebApiEndpoints.GetToken(_username, _password)).GetAwaiter().GetResult();

            //if (response.Status == System.Net.HttpStatusCode.OK)
            //    throw new Exception("Status not ok");

            //HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.Payload);
        }

        #region Response handlers
        public Response HandleResponse(HttpResponseMessage responseMessage)
        {
            var response = new Response();
            response.Status = responseMessage.StatusCode;
            return response;
        }
        public Response<TPayload> HandleResponse<TPayload>(HttpResponseMessage responseMessage)
        {
            var response = new Response<TPayload>();
            response.Status = responseMessage.StatusCode;
            response.Payload = JsonConvert.DeserializeObject<TPayload>(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            return response;
        }
        public async Task<Response<TPayload>> HandleResponseAsync<TPayload>(HttpResponseMessage responseMessage)
        {
            var response = new Response<TPayload>();
            response.Status = responseMessage.StatusCode;
            string asd = await responseMessage.Content.ReadAsStringAsync();
            response.Payload = JsonConvert.DeserializeObject<TPayload>(await responseMessage.Content.ReadAsStringAsync());
            return response;
        }
        public async Task<Response<string>> HandleResponseAsRawString<TRawPayload>(HttpResponseMessage responseMessage)
        {
            var response = new Response<string>();
            response.Status = responseMessage.StatusCode;
            response.Payload = await responseMessage.Content.ReadAsStringAsync();
            return response;
        }
        #endregion

        #region Get
        public async Task<Response> GetAsync(string endpoint)
        {
            return HandleResponse(await HttpClient.GetAsync(endpoint));
        }
        public async Task<Response<TPayload>> GetAsync<TPayload>(string endpoint)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.GetAsync(endpoint));
        }
        #endregion

        #region Post
        public async Task<Response> PostAsync(string endpoint)
        {
            return HandleResponse(await HttpClient.PostAsync(endpoint, null));
        }
        public async Task<Response<TPayload>> PostAsync<TPayload>(string endpoint)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PostAsync(endpoint, null));
        }
        public async Task<Response<string>> PostRawResponseStringAsync(string endpoint)
        {
            return await HandleResponseAsRawString<string>(await HttpClient.PostAsync(endpoint, null));

        }

        public async Task<Response> PostAsync<TRequest>(string endpoint, TRequest request)
        {
            return HandleResponse(await HttpClient.PostAsJsonAsync<TRequest>(endpoint, request));
        }
        public async Task<Response<TPayload>> PostAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PostAsJsonAsync<TRequest>(endpoint, request));
        }

        public async Task<Response> PostAsync<TRequest>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return HandleResponse(await httpClient.PostAsJsonAsync(endpoint, request));
        }
        public async Task<Response<TPayload>> PostAsync<TRequest, TPayload>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await httpClient.PostAsJsonAsync(endpoint, request));
        }
        #endregion
    }
}
