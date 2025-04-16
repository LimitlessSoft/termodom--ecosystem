using LSCore.Common.Contracts;

namespace TD.Office.Public.Contracts.Requests.Users;

public class UpdateStoreIdRequest : LSCoreIdRequest
{
	public int? StoreId { get; set; }
}
