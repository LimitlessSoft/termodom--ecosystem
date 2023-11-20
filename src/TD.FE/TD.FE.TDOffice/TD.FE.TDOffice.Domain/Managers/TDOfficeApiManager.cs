using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class TDOfficeApiManager : LSCoreBaseApiManager, ITDOfficeApiManager
    {
        public TDOfficeApiManager(ILogger<TDOfficeApiManager> logger) : base()
        {
#if DEBUG
            HttpClient.BaseAddress = new Uri("http://localhost:32778");
#else
            string? serverHost = Environment.GetEnvironmentVariable("API_HOST");

            if (string.IsNullOrWhiteSpace(serverHost))
            {
                logger.LogCritical("Environment variable 'API_HOST' is not assigned!");
                throw new ArgumentNullException(nameof(serverHost));
            }
            HttpClient.BaseAddress = new Uri($"http://{serverHost}:32778");
#endif
        }
    }
}
