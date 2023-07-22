using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using TDOffice_v2.Core.Http;
using System.Net.Http.Json;

namespace TDOffice_v2
{
    public static class TDAPI
    {
        public static HttpClient HttpClient { get; set; } = new HttpClient()
        {
#if DEBUG
            BaseAddress = new Uri("http://localhost:5085")
#else
            BaseAddress = new Uri("http://4monitor:32779")
#endif
        };

#region Response handlers
        public static Response<TPayload> HandleResponse<TPayload>(HttpResponseMessage responseMessage)
        {
            if (responseMessage.NotOk())
                return Response<TPayload>.BadRequest();

            return JsonConvert.DeserializeObject<Response<TPayload>>(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
        public static async Task<Response<TPayload>> HandleResponseAsync<TPayload>(HttpResponseMessage responseMessage)
        {
            if (responseMessage.NotOk())
                return Response<TPayload>.BadRequest();

            return JsonConvert.DeserializeObject<Response<TPayload>>(await responseMessage.Content.ReadAsStringAsync());
        }
        public static Response HandleRawResponse(HttpResponseMessage responseMessage)
        {
            var response = new Response();
            response.Status = responseMessage.StatusCode;
            return response;
        }
        public static Response<TPayload> HandleRawResponse<TPayload>(HttpResponseMessage responseMessage)
        {
            var response = new Response<TPayload>();
            response.Status = responseMessage.StatusCode;
            response.Payload = JsonConvert.DeserializeObject<TPayload>(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            return response;
        }
        public static async Task<Response<TPayload>> HandleRawResponseAsync<TPayload>(HttpResponseMessage responseMessage)
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
        public static async Task<Response<TPayload>> GetAsync<TPayload>(string endpoint)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.GetAsync(endpoint));
        }
        public static async Task<Response<TPayload>> GetAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.GetAsJsonAsync<TRequest>(endpoint, request));
        }
        public static async Task<Response> GetRawAsync(string endpoint)
        {
            return HandleRawResponse(await HttpClient.GetAsync(endpoint));
        }
        public static async Task<Response<TPayload>> GetRawAsync<TPayload>(string endpoint)
        {
            return await HandleRawResponseAsync<TPayload>(await HttpClient.GetAsync(endpoint));
        }
        public static async Task<Response<TPayload>> GetRawAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleRawResponseAsync<TPayload>(await HttpClient.GetAsJsonAsync<TRequest>(endpoint, request));
        }
#endregion

#region Post
        public static async Task<Response> PostRawAsync(string endpoint)
        {
            return HandleRawResponse(await HttpClient.PostAsync(endpoint, null));
        }
        public static async Task<Response<TPayload>> PostAsync<TPayload>(string endpoint)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PostAsync(endpoint, null));
        }
        public static async Task<Response<TPayload>> PostRawAsync<TPayload>(string endpoint)
        {
            return await HandleRawResponseAsync<TPayload>(await HttpClient.PostAsync(endpoint, null));
        }

        public static async Task<Response> PostAsync<TRequest>(string endpoint, TRequest request)
        {
            return HandleRawResponse(await HttpClient.PostAsJsonAsync<TRequest>(endpoint, request));
        }
        public static async Task<Response<TPayload>> PostAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PostAsJsonAsync<TRequest>(endpoint, request));
        }

        public static async Task<Response> PostAsync<TRequest>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return HandleRawResponse(await httpClient.PostAsJsonAsync(endpoint, request));
        }
        public static async Task<Response<TPayload>> PostAsync<TRequest, TPayload>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await httpClient.PostAsJsonAsync(endpoint, request));
        }
        #endregion
        #region Put

        public static async Task<Response> PutRawAsync(string endpoint)
        {
            return HandleRawResponse(await HttpClient.PutAsync(endpoint, null));
        }
        public static async Task<Response<TPayload>> PutAsync<TPayload>(string endpoint)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PutAsync(endpoint, null));
        }
        public static async Task<Response<TPayload>> PutRawAsync<TPayload>(string endpoint)
        {
            return await HandleRawResponseAsync<TPayload>(await HttpClient.PutAsync(endpoint, null));
        }

        public static async Task<Response> PutAsync<TRequest>(string endpoint, TRequest request)
        {
            return HandleRawResponse(await HttpClient.PutAsJsonAsync<TRequest>(endpoint, request));
        }
        public static async Task<Response<TPayload>> PutAsync<TRequest, TPayload>(string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await HttpClient.PutAsJsonAsync<TRequest>(endpoint, request));
        }

        public static async Task<Response> PutAsync<TRequest>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return HandleRawResponse(await httpClient.PutAsJsonAsync(endpoint, request));
        }
        public static async Task<Response<TPayload>> PutAsync<TRequest, TPayload>(HttpClient httpClient, string endpoint, TRequest request)
        {
            return await HandleResponseAsync<TPayload>(await httpClient.PutAsJsonAsync(endpoint, request));
        }
        #endregion
    }
}
