using System.Text.Json.Serialization;

namespace TD.Web.Common.Contracts.Requests.Users;

public class SetUserProductPriceGroupLevelRequest
{
	public long? Id { get; set; }

	[JsonIgnore]
	public int? ProductPriceGroupId { get; set; }
	public int? Level { get; set; }
}
