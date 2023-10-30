using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Extensions;
using TD.Core.Contracts.Http;
using TD.Core.Framework;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("/login")]
        public Response<string> Login([FromBody]UserLoginRequest request)
        {
            return _userManager.Login(request);
        }

        [HttpPut]
        [Route("/register")]
        public Response Register([FromBody]UserRegisterRequest request)
        {
            return _userManager.Register(request);
        }

        [HttpPut]
        [Route("/users/{id}/promote")]
        [Authorization(UserType.SuperAdmin)]
        public Response PromoteUser([FromRoute] int id, [FromBody] UserPromoteRequest request)
        {
            if (request.IdsNotMatch(id))
                return Core.Contracts.Http.Response.BadRequest();

            return _userManager.PromoteUser(request);
        }

        [HttpPut]
        [Route("/users/{id}/product-price-group-levels/{ProductPriceGroupId}")]
        public Response SetUserProductPriceGroupLevel([FromRoute] int id, [FromRoute] int ProductPriceGroupId, [FromBody] SetUserProductPriceGroupLevelRequest request)
        {
            if (request.IdsNotMatch(id))
                return Core.Contracts.Http.Response.BadRequest();

            request.Id = id;
            request.ProductPriceGroupId = ProductPriceGroupId;

            return _userManager.SetUserProductPriceGroupLevel(request);
        }
    }
}
