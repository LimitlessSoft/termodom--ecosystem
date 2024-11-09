using LSCore.Contracts.Requests;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;

namespace TD.Office.Public.Contracts.Requests.Partneri;
public class GetPartnersReportByYearsKomercijalnoFinansijskoRequest
    : LSCoreSortableAndPageableRequest<PartnersSortColumnsCodes.Partners>
{
    public string? SearchKeyword { get; set; }
    public int[] Years {  get; set; }
    public double Tolerancija { get; set; }
}
