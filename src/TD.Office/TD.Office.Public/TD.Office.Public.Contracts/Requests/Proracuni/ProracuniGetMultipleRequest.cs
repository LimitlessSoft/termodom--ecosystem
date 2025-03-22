using LSCore.SortAndPage.Contracts;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniGetMultipleRequest
	: LSCoreSortableAndPageableRequest<ProracuniSortColumnCodes.Proracuni>
{
	public DateTime FromUtc { get; set; }
	public DateTime ToUtc { get; set; }
	public int? MagacinId { get; set; }
}
