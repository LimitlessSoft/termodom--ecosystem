using LSCore.Contracts.Requests;
using TD.Komercijalno.Contracts.Enums.SortColumnCodes;

namespace TD.Komercijalno.Contracts.Requests.Partneri;

public class PartneriGetMultipleRequest
    : LSCoreSortableAndPageableRequest<PartneriSortColumCodes.Partneri>
{
    public string? SearchKeyword { get; set; }
    public string? Pib { get; set; }
    public string? Mbroj { get; set; }
}
