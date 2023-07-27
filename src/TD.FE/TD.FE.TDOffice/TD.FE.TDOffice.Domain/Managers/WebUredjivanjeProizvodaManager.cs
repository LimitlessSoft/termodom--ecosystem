using Microsoft.Extensions.Logging;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class WebUredjivanjeProizvodaManager : BaseManager<WebUredjivanjeProizvodaManager>, IWebUredjivanjeProizvodaManager
    {
        public WebUredjivanjeProizvodaManager(ILogger<WebUredjivanjeProizvodaManager> logger)
            : base(logger)
        {
        }
    }
}
