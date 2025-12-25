using TD.Office.Public.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Popisi;

public class PopisMasovnoDodavanjeStavkiRequest
{
	public PopisMasovnoDodavanjeStavkiActionType ActionType { get; set; }
	public string? Tag { get; set; }
}
