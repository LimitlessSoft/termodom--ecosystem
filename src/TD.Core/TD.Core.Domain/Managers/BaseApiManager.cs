using Newtonsoft.Json;
using System.Net.Http.Json;
using TD.Core.Contracts.Http;
using static TD.Core.Contracts.Http.Extensions;
using TD.Core.Contracts.IManagers;
using System.ComponentModel;
using TD.Core.Contracts.Extensions;

namespace TD.Core.Domain.Managers
{
    public abstract class BaseApiManager : IApiManager
    {
        public HttpClient HttpClient { get; set; }

        public BaseApiManager()
        {
            HttpClient = new HttpClient();
        }

        #region Response handlers
        public Response<TPayload> HandleResponse<TPayload>(HttpResponseMessage responseMessage)
        {
            if (responseMessage.NotOk())
                return Response<TPayload>.BadRequest();

            return JsonConvert.DeserializeObject<Response<TPayload>>(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
        public async Task<Response<TPayload>> HandleResponseAsync<TPayload>(HttpResponseMessage responseMessage)
        {
            if (responseMessage.NotOk())
                return Response<TPayload>.BadRequest();

            var responseString = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<TPayload>>(responseString);
        }
        public Response HandleRawResponse(HttpResponseMessage responseMessage)
        {
            var response = new Response();
            response.Status = responseMessage.StatusCode;
            return response;
        }
        public Response<TPayload> HandleRawResponse<TPayload>(HttpResponseMessage responseMessage)
        {
            var response = new Response<TPayload>();
            response.Status = responseMessage.StatusCode;
            response.Payload = JsonConvert.DeserializeObject<TPayload>(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            return response;
        }
        public async Task<Response<TPayload>> HandleRawResponseAsync<TPayload>(HttpResponseMessage responseMessage)
        {
            var response = new Response<TPayload>();
            response.Status = responseMessage.StatusCode;
            var converter = TypeDescriptor.GetConverter(typeof(TPayload));
            var content = await responseMessage.Content.ReadAsStringAsync();

            if (content[0] == '{' || content[0] == '[')
                response.Payload = JsonConvert.DeserializeObject<TPayload>(await responseMessage.Content.ReadAsStringAsync());
            else
                response.Payload = (TPayload)Convert.ChangeType(await responseMessage.Content.ReadAsStringAsync(), typeof(TPayload));
            return response;
        }
        #endregion

        #region Get
        public async Task<Response<TPayload>> GetAsync<TPayload>(string endpoint)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.GetAsync(endpoint));
        }
        public async Task<Response<TPayload>> GetAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.GetAsJsonAsync<TRequest>(endpoint, request));
        }
        public async Task<Response> GetRawAsync(string endpoint)
        {
            return HandleRawResponse(await HttpClient.GetAsync(endpoint));
        }
        public async Task<Response<TPayload>> GetRawAsync<TPayload>(string endpoint)
        {
            return await HandleRawResponseAsync<TPayload>(await HttpClient.GetAsync(endpoint));
        }
        public async Task<Response<TPayload>> GetRawAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleRawResponseAsync<TPayload>(await HttpClient.GetAsJsonAsync<TRequest>(endpoint, request));
        }
        #endregion

        #region Post
        public async Task<Response> PostRawAsync(string endpoint)
        {
            return HandleRawResponse(await HttpClient.PostAsync(endpoint, null));
        }
        public async Task<Response<TPayload>> PostAsync<TPayload>(string endpoint)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PostAsync(endpoint, null));
        }
        public async Task<Response<TPayload>> PostRawAsync<TPayload>(string endpoint)
        {
            return await HandleRawResponseAsync<TPayload>(await HttpClient.PostAsync(endpoint, null));
        }

        public async Task<Response> PostAsync<TRequest>(string endpoint, TRequest request)
        {
            return HandleRawResponse(await HttpClient.PostAsJsonAsync<TRequest>(endpoint, request));
        }
        public async Task<Response<TPayload>> PostAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PostAsJsonAsync<TRequest>(endpoint, request));
        }

        public async Task<Response> PostAsync<TRequest>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return HandleRawResponse(await httpClient.PostAsJsonAsync(endpoint, request));
        }
        public async Task<Response<TPayload>> PostAsync<TRequest, TPayload>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await httpClient.PostAsJsonAsync(endpoint, request));
        }
        #endregion

        #region Put

        public async Task<Response> PutRawAsync(string endpoint)
        {
            return HandleRawResponse(await HttpClient.PutAsync(endpoint, null));
        }
        public async Task<Response<TPayload>> PutAsync<TPayload>(string endpoint)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PutAsync(endpoint, null));
        }
        public async Task<Response<TPayload>> PutRawAsync<TPayload>(string endpoint)
        {
            return await HandleRawResponseAsync<TPayload>(await HttpClient.PutAsync(endpoint, null));
        }

        public async Task<Response> PutAsync<TRequest>(string endpoint, TRequest request)
        {
            return HandleRawResponse(await HttpClient.PutAsJsonAsync<TRequest>(endpoint, request));
        }
        public async Task<Response<TPayload>> PutAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PutAsJsonAsync<TRequest>(endpoint, request));
        }

        public async Task<Response> PutAsync<TRequest>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return HandleRawResponse(await httpClient.PutAsJsonAsync(endpoint, request));
        }
        public async Task<Response<TPayload>> PutAsync<TRequest, TPayload>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await httpClient.PutAsJsonAsync(endpoint, request));
        }
        #endregion

        #region Delete
        public async Task<Response> DeleteAsync(string endpoint)
        {
            return HandleRawResponse(await HttpClient.DeleteAsync(endpoint));
        }
        #endregion
    }
}
