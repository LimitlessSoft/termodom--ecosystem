using LSCore.Common.Contracts;
using TD.Office.Public.Contracts.Dtos.Odsustvo;
using TD.Office.Public.Contracts.Requests.Odsustvo;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IOdsustvoManager
{
	List<OdsustvoCalendarDto> GetCalendar(GetOdsustvoCalendarRequest request);
	List<OdsustvoCalendarDto> GetYearList(GetOdsustvoYearListRequest request);
	List<OdsustvoCalendarDto> GetPending();
	OdsustvoDto GetSingle(LSCoreIdRequest request);
	void Save(SaveOdsustvoRequest request);
	void Delete(long id);
	void Approve(long id);
	void UpdateRealizovano(long id, UpdateRealizovanoRequest request);
}
