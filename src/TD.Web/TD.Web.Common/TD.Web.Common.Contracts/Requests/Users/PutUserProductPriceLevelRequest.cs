using LSCore.Contracts.Requests;

namespace TD.Web.Common.Contracts.Requests.Users
{
    public class PutUserProductPriceLevelRequest
    {
        public int UserId { get; set; }
        public int Level {  get; set; }
        public int ProductPriceGroupId { get; set; }
    }
}