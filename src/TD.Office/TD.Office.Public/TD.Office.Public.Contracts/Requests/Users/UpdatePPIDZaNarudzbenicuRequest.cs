using LSCore.Common.Contracts;

namespace TD.Office.Public.Contracts.Requests.Users;

public class UpdatePPIDZaNarudzbenicuRequest : LSCoreIdRequest
{
	public int? PPIDZaNarudzbenicu { get; set; }
}
