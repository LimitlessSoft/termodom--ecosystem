using TD.Core.Contracts.Http;

namespace TD.Core.Contracts.IManagers
{
    public interface IApiManager
    {
        HttpClient HttpClient { get; set; }
        Response HandleResponse(HttpResponseMessage responseMessage);
        Response<TPayload> HandleResponse<TPayload>(HttpResponseMessage responseMessage);
        Task<Response<TPayload>> HandleResponseAsync<TPayload>(HttpResponseMessage responseMessage);
        Task<Response> GetAsync(string endpoint);
        Task<Response<TPayload>> GetAsync<TPayload>(string endpoint);
        Task<Response> PostAsync(string endpoint);
        Task<Response<TPayload>> PostAsync<TPayload>(string endpoint);
        Task<Response<string>> PostRawResponseStringAsync(string endpoint);
        Task<Response> PostAsync<TRequest>(string endpoint, TRequest request);
        Task<Response<TPayload>> PostAsync<TRequest, TPayload>(string endpoint, TRequest request);
        Task<Response> PostAsync<TRequest>(HttpClient httpClient, string endpoint, TRequest request);
        Task<Response<TPayload>> PostAsync<TRequest, TPayload>(HttpClient httpClient, string endpoint, TRequest request);
    }
}
