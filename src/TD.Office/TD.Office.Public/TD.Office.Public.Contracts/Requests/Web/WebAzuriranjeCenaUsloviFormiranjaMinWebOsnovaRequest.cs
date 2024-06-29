using TD.Office.Common.Contracts.Enums;
using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Web;

public class WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest : LSCoreSaveRequest
{
    public long WebProductId { get; set; }
    public UslovFormiranjaWebCeneType Type { get; set; }
    public decimal Modifikator { get; set; }
}