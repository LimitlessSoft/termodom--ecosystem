namespace TDBrain_v3
{
    public class TDWebAPI
    {
        private static readonly string _baseUrl = "https://api.termodom.rs";
        //private static readonly string _baseUrl = "https://localhost:44311";
        private static readonly string _APIUsername = "TDBrain_v3";
        private static readonly string _APIPassword = "Plivanje123$";
        private static string? _bearerToken = null;
        private static readonly HttpClient _client = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
        private static readonly int _requestTries = 10;

        static TDWebAPI()
        {
            _ = LoginAsync();
        }

        private static async Task LoginAsync()
        {
            var response = await PostAsync("/api/Korisnik/GetToken?username=" + _APIUsername + "&password=" + _APIPassword);
            _bearerToken = await response.Content.ReadAsStringAsync();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", _bearerToken);
        }

        public static async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            int currTry = 0;
            var resp = await _client.GetAsync(_baseUrl + endpoint);
            while ((int)resp.StatusCode == 403)
            {
                currTry++;
                if (currTry > _requestTries)
                    throw new Exception("Premasio sam maksimalni broj pokusaja autorizacije na termdom API!");

                await LoginAsync();
                resp = await _client.GetAsync(_baseUrl + endpoint);
            }
            return resp;
        }
        public static Task<HttpResponseMessage> PostAsync(string endpoint)
        {
            return Task.Run(async () =>
            {
                int currTry = 0;
                var resp = await _client.PostAsync(_baseUrl + endpoint, null);
                while ((int)resp.StatusCode == 403)
                {
                    currTry++;
                    if(currTry > _requestTries)
                        throw new Exception("Premasio maksimalni broj pokusaja autorizacije na termdom API!");

                    await LoginAsync();
                    resp = await _client.PostAsync(_baseUrl + endpoint, null);
                }
                return resp;
            });
        }
        public static Task<HttpResponseMessage> PostAsync(string endpoint, Dictionary<string, string> parameters)
        {
            return Task.Run(async () =>
            {
                var content = new FormUrlEncodedContent(parameters);
                int currTry = 0;
                var resp = await _client.PostAsync(_baseUrl + endpoint, content);
                while ((int)resp.StatusCode == 403)
                {
                    currTry++;
                    if (currTry > _requestTries)
                        throw new Exception("Premasio maksimalni broj pokusaja autorizacije na termdom API!");

                    await LoginAsync();
                    resp = await _client.PostAsync(_baseUrl + endpoint, content);
                }
                return resp;
            });
        }
        public static Task<HttpResponseMessage> PostAsync(string endpoint, string stringContent)
        {
            return Task.Run(async () =>
            {
                var content = new StringContent(stringContent);
                int currTry = 0;
                var resp = await _client.PostAsync(_baseUrl + endpoint, content);
                while ((int)resp.StatusCode == 403)
                {
                    currTry++;
                    if (currTry > _requestTries)
                        throw new Exception("Premasio maksimalni broj pokusaja autorizacije na termdom API!");

                    await LoginAsync();
                    resp = await _client.PostAsync(_baseUrl + endpoint, content);
                }
                return resp;
            });
        }
    }
}
