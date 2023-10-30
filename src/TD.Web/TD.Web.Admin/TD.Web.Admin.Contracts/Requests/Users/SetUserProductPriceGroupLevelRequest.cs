using System.Text.Json.Serialization;
using TD.Core.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Users
{
    public class SetUserProductPriceGroupLevelRequest : SaveRequest
    {
        [JsonIgnore]
        public int? ProductPriceGroupId { get; set; }
        public int? Level { get; set; }
    }
}
