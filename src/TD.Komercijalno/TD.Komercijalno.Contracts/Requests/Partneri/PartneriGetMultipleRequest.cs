using LSCore.SortAndPage.Contracts;
using TD.Komercijalno.Contracts.Enums.SortColumnCodes;

namespace TD.Komercijalno.Contracts.Requests.Partneri;

public class PartneriGetMultipleRequest
	: LSCoreSortableAndPageableRequest<PartneriSortColumCodes.Partneri>
{
	public string? SearchKeyword { get; set; }
	public string? Pib { get; set; }
	public string? Mbroj { get; set; }
	public int[]? Ppid { get; set; }
	public short? Aktivan { get; set; }
}
