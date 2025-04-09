using TD.Office.MassSMS.Contracts.Requests;

namespace TD.Office.MassSMS.Contracts.Interfaces;

public interface IMassSMSManager
{
	void InvokeSending();
	void Queue(QueueSmsRequest request);
	string GetCurrentStatus();
	int GetQueueCount();
}
