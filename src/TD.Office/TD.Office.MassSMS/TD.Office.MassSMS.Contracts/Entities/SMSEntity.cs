using LSCore.Repository.Contracts;
using TD.Office.MassSMS.Contracts.Enums;

namespace TD.Office.MassSMS.Contracts.Entities;

public class SMSEntity : LSCoreEntity
{
	public string Text { get; set; }
	public string Phone { get; set; }
	public SMSStatus Status { get; set; }
}
