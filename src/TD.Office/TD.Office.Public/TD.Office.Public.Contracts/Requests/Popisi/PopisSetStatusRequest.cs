using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Popisi;

public class PopisSetStatusRequest
{
	public long Id { get; set; }
	public DokumentStatus Status { get; set; }
}
