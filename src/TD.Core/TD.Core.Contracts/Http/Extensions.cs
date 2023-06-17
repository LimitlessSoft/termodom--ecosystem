namespace TD.Core.Contracts.Http
{
    public static class Extensions
    {
        public static Task<HttpResponseMessage> GetAsJsonAsync<TRequest>(this HttpClient httpClient, string endpoint, TRequest request)
        {
            string finalEndpoint = endpoint + "?";
            foreach (var propertyInfo in request.GetType().GetProperties())
                finalEndpoint += $"{propertyInfo.Name}={propertyInfo.GetValue(request)}";

            return httpClient.GetAsync(finalEndpoint);
        }
    }
}
