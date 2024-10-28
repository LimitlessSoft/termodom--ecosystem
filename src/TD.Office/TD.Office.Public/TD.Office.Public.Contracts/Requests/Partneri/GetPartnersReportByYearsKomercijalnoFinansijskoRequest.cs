using LSCore.Contracts.Requests;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;

namespace TD.Office.Public.Contracts.Requests.Partneri
{
    public class GetPartnersReportByYearsKomercijalnoFinansijskoRequest
        : LSCoreSortableAndPageableRequest<PartnersSortColumnsCodes.Partners>
    {
        public int[] Years {  get; set; }
    }
}
