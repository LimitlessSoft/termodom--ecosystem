using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class TDOfficeApiManager : BaseApiManager, ITDOfficeApiManager
    {
        public TDOfficeApiManager() : base()
        {
#if DEBUG
            HttpClient.BaseAddress = new Uri("https://localhost:555");
#else
            HttpClient.BaseAddress = new Uri("http://192.168.0.11:32776");
#endif
        }
    }
}
