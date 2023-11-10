using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class TDWebVeleprodajaApiManager : LSCoreBaseApiManager, ITDWebVeleprodajaApiManager
    {
        public TDWebVeleprodajaApiManager(ILogger<TDWebVeleprodajaApiManager> logger) : base()
        {
#if DEBUG
            HttpClient.BaseAddress = new Uri("http://localhost:33770");
#else
            string? serverHost = Environment.GetEnvironmentVariable("API_HOST");

            if (string.IsNullOrWhiteSpace(serverHost))
            {
                logger.LogCritical("Environment variable 'API_HOST' is not assigned!");
                throw new ArgumentNullException(nameof(serverHost));
            }
            HttpClient.BaseAddress = new Uri($"http://{serverHost}:33770");
#endif
        }
    }
}
