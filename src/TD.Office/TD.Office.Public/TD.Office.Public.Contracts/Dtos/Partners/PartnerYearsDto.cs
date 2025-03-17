using TD.Web.Common.Contracts.Dtos;

namespace TD.Office.Public.Contracts.Dtos.Partners;

public class PartnerYearsDto
{
	public List<PartnerYearDto> Years { get; set; }
	public int DefaultTolerancija { get; set; }
	public List<IdNamePairDto> Status { get; set; }
}
