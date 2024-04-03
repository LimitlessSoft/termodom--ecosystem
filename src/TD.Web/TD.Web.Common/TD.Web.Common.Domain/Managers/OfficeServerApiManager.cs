using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Common.Domain.Managers
{
    public class OfficeServerApiManager : LSCoreBaseApiManager, IOfficeServerApiManager
    {
        public OfficeServerApiManager()
        {
            base.HttpClient.BaseAddress = new Uri(Contracts.Constants.OfficeServerApiUrl);
        }
        
        public Task<LSCoreResponse> SMSQueueAsync(SMSQueueRequest request) =>
            PostAsync<SMSQueueRequest>($"/SMS/Queue", request);
    }
}