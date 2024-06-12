using TD.OfficeServer.Contracts.Requests.SMS;

namespace TD.OfficeServer.Contracts.IManagers
{
    public interface ISmsManager
    {
        string ConnectionString { get; set; }
        void Queue(SMSQueueRequest request);
    }
}
