using TD.Core.Domain.Managers;
using TD.WebshopListener.Contracts.Constants;
using TD.WebshopListener.Contracts.IManagers;

namespace TD.WebshopListener.Domain.Managers
{
    public class WebshopApiManager : ApiManager, IWebshopApiManager
    {
        private readonly string _username = "td-webshop-listener";
        private readonly string _password = "Plivanje123$";
        public WebshopApiManager() : base()
        {
            HttpClient.BaseAddress = new Uri("https://api.termodom.rs");
            var response = PostRawResponseStringAsync(WebApiEndpoints.GetToken(_username, _password)).GetAwaiter().GetResult();

            if (response.Status != System.Net.HttpStatusCode.OK)
                throw new Exception("Status not ok");

            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.Payload);
        }
    }
}
