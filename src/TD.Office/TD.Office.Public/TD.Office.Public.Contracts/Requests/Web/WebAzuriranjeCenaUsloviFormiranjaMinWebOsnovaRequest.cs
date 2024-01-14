using LSCore.Contracts.Requests;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Web
{
    public class WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest : LSCoreSaveRequest
    {
        public int WebProductId { get; set; }
        public UslovFormiranjaWebCeneType Type { get; set; }
        public decimal Modifikator { get; set; }
    }
}
