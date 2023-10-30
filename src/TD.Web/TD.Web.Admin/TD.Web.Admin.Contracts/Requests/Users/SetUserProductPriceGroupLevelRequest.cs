using System.Text.Json.Serialization;
using TD.Core.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Users
{
    public class SetUserProductPriceGroupLevelRequest : SaveRequest
    {
        public int Level { get; set; }

        [JsonIgnore]
        public int ProductPriceGroupId { get; set; }
    }
}
