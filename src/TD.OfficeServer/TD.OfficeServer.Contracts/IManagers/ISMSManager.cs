using LSCore.Contracts.Http;
using TD.OfficeServer.Contracts.Requests.SMS;

namespace TD.OfficeServer.Contracts.IManagers
{
    public interface ISMSManager
    {
        string ConnectionString { get; set; }
        LSCoreResponse Queue(SMSQueueRequest request);
    }
}
