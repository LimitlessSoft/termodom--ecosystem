using TD.OfficeServer.Contracts.Requests.SMS;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IOfficeServerApiManager
{
    Task SmsQueueAsync(SMSQueueRequest request);
}