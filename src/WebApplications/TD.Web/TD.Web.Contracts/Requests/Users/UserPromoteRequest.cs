using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Requests.Users
{
    public class UserPromoteRequest
    {
        [FromRoute]
        public int Id { get; set; }
        [FromBody]
        public UserType UserType { get; set; }
    }
}
