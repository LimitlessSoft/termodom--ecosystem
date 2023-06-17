using TD.Core.Domain.Managers;
using TD.WebshopListener.Contracts.IManagers;

namespace TD.WebshopListener.Domain.Managers
{
    public class TDBrainApiManager : ApiManager, ITDBrainApiManager
    {
        public TDBrainApiManager() : base()
        {
            HttpClient.BaseAddress = new Uri("http://192.168.0.3:32775");
        }
    }
}
