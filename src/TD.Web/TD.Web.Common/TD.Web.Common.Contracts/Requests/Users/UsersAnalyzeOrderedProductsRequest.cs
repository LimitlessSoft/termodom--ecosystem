using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Users
{
    public class UsersAnalyzeOrderedProductsRequest
    {
        public string Username { get; set; }
        public UsersAnalyzeOrderedProductsRange Range { get; set; }
    }
}