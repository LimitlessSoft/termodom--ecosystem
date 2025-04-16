using LSCore.Common.Contracts;

namespace TD.Office.Public.Contracts.Requests.Users;

public class UpdateVPMagacinIdRequest : LSCoreIdRequest
{
	public int? VPMagacinId { get; set; }
}
