using TD.TDOffice.Contracts.Dtos.Users;
using TD.TDOffice.Contracts.IManagers;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace TD.TDOffice.Api.Controllers;

[ApiController]
public class UsersController (IUserManager userManager) : Controller
{
    [HttpGet]
    [Route("/users/{id}")]
    public UserDto Get([FromRoute]LSCoreIdRequest request) =>
        userManager.Get(request);

    [HttpGet]
    [Route("/users")]
    public List<UserDto> GetMultiple() =>
        userManager.GetMultiple();
}