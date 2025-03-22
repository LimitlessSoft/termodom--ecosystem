using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Web;

public class WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest
{
	public long? Id { get; set; }
	public long WebProductId { get; set; }
	public UslovFormiranjaWebCeneType Type { get; set; }
	public decimal Modifikator { get; set; }
}
