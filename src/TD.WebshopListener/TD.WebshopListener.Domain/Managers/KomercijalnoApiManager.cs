using LSCore.Domain.Managers;
using TD.WebshopListener.Contracts.IManagers;

namespace TD.WebshopListener.Domain.Managers
{
    public class KomercijalnoApiManager : LSCoreBaseApiManager, IKomercijalnoApiManager
    {
        public KomercijalnoApiManager() : base()
        {
#if DEBUG
            HttpClient.BaseAddress = new Uri("https://localhost:44341");
#else
            HttpClient.BaseAddress = new Uri("http://192.168.0.11:32776");
#endif
        }
    }
}
