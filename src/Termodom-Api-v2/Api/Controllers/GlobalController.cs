using Api.Interfaces.Managers;
using Api.RequestBodies;
using Infrastructure.Framework;
using Infrastructure.Framework.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    public class GlobalController : Controller
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IUsersManager _usersManager;

        public GlobalController(IConfigurationRoot configuration, IUsersManager usersManager)
        {
            _usersManager = usersManager;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/authorize")]
        public IAPIResponse Authorize([FromBody] AuthorizeRequestBody requestBody)
        {
            if (!_usersManager.Authenticate(requestBody.Username, requestBody.Password))
                return new APIResponse(HttpStatusCode.Unauthorized);

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, requestBody.Username),
                    new Claim(JwtRegisteredClaimNames.Email, requestBody.Username),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return new APIResponse<string>(stringToken);
        }

    }
}
