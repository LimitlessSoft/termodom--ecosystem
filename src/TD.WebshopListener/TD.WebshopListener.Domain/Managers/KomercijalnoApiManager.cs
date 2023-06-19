using TD.Core.Domain.Managers;
using TD.WebshopListener.Contracts.IManagers;

namespace TD.WebshopListener.Domain.Managers
{
    public class KomercijalnoApiManager : BaseApiManager, IKomercijalnoApiManager
    {
        public KomercijalnoApiManager() : base()
        {
            HttpClient.BaseAddress = new Uri("http://192.168.0.11:32776");
        }
    }
}
