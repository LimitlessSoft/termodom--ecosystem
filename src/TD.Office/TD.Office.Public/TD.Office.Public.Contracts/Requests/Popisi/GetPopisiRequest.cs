using LSCore.SortAndPage.Contracts;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;

namespace TD.Office.Public.Contracts.Requests.Popisi;

public class GetPopisiRequest : LSCoreSortableAndPageableRequest<PopisiSortColumnCodes.Popisi>
{
	public DateTime? FromDate { get; set; }
	public DateTime? ToDate { get; set; }
	public long? MagacinId { get; set; }
}
