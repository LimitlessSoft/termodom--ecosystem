using Api.Interfaces.Managers;
using Infrastructure.Entities.ApiV2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IUsersManager _usersManager;

        public UsersController(IConfigurationRoot configuration, IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        [Authorize]
        [Route("/Users")]
        public IQueryable<User> List()
        {
            return _usersManager.List();
        }
    }
}
