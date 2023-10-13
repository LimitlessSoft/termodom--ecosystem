using TD.Core.Contracts.Http;

namespace TD.Core.Contracts.IManagers
{
    public interface IApiManager
    {
        HttpClient HttpClient { get; set; }
        Response<TPayload> HandleResponse<TPayload>(HttpResponseMessage responseMessage);
        Task<Response<TPayload>> HandleResponseAsync<TPayload>(HttpResponseMessage responseMessage);
        Response HandleRawResponse(HttpResponseMessage responseMessage);
        Response<TPayload> HandleRawResponse<TPayload>(HttpResponseMessage responseMessage);
        Task<Response<TPayload>> HandleRawResponseAsync<TPayload>(HttpResponseMessage responseMessage);
        Task<Response<TPayload>> GetAsync<TPayload>(string endpoint);
        Task<Response<TPayload>> GetAsync<TRequest, TPayload>(string endpoint, TRequest request);
        Task<Response> GetRawAsync(string endpoint);
        Task<Response<TPayload>> GetRawAsync<TPayload>(string endpoint);
        Task<Response<TPayload>> GetRawAsync<TRequest, TPayload>(string endpoint, TRequest request);
        Task<Response> PostRawAsync(string endpoint);
        Task<Response> PutRawAsync(string endpoint);
        Task<Response<TPayload>> PutAsync<TPayload>(string endpoint);
        Task<Response<TPayload>> PostAsync<TPayload>(string endpoint);
        Task<Response<TPayload>> PutRawAsync<TPayload>(string endpoint);
        Task<Response<TPayload>> PostRawAsync<TPayload>(string endpoint);
        Task<Response> PostAsync<TRequest>(string endpoint, TRequest request);
        Task<Response> PutAsync<TRequest>(string endpoint, TRequest request);
        Task<Response<TPayload>> PostAsync<TRequest, TPayload>(string endpoint, TRequest request);
        Task<Response<TPayload>> PutAsync<TRequest, TPayload>(string endpoint, TRequest request);
        Task<Response> PostAsync<TRequest>(HttpClient httpClient, string endpoint, TRequest request);
        Task<Response> PutAsync<TRequest>(HttpClient httpClient, string endpoint, TRequest request);
        Task<Response<TPayload>> PostAsync<TRequest, TPayload>(HttpClient httpClient, string endpoint, TRequest request);
        Task<Response<TPayload>> PutAsync<TRequest, TPayload>(HttpClient httpClient, string endpoint, TRequest request);
        Task<Response> DeleteAsync(string endpoint);
    }
}
