using TD.Office.MassSMS.Contracts.Dtos;
using TD.Office.MassSMS.Contracts.Requests;

namespace TD.Office.MassSMS.Contracts.Interfaces;

public interface IMassSMSManager
{
	void InvokeSending();
	void Queue(QueueSmsRequest request);
	string GetCurrentStatus();
	int GetQueueCount();
	List<SMSDto> GetQueue();
	void ClearQueue();
	void MassQueue(MassQueueSmsRequest request);
	void ClearDuplicates();
	void SetText(SetTextRequest request);
	void ClearBlacklisted();
	bool IsBlacklisted(string number);
	void Blacklist(string number);
}
