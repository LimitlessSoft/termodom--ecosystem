using LSCore.Contracts.Requests;
using System.Text.Json.Serialization;

namespace TD.Web.Admin.Contracts.Requests.Users
{
    public class SetUserProductPriceGroupLevelRequest : LSCoreSaveRequest
    {
        [JsonIgnore]
        public int? ProductPriceGroupId { get; set; }
        public int? Level { get; set; }
    }
}
