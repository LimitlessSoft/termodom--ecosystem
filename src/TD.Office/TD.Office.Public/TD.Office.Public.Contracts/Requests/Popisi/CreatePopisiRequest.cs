using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Popisi;

public class CreatePopisiRequest
{
	public PopisDokumentType Type { get; set; }
	public PopisDokumentTime Time { get; set; }
}
