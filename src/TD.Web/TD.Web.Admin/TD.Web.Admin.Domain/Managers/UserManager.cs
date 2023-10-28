using Microsoft.Extensions.Logging;
using System.Text;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Users;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TD.Core.Domain.Validators;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Enums;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Domain.Managers
{
    public class UserManager : BaseManager<UserManager, UserEntity>, IUserManager
    {
        private readonly IConfigurationRoot _configurationRoot;
        public UserManager(IConfigurationRoot configurationRoot, ILogger<UserManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
            _configurationRoot = configurationRoot;
        }

        private string GenerateJSONWebToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
#if DEBUG
                _configurationRoot["Jwt:Key"]
#else
                _configurationRoot["JWT_KEY"]
#endif
                ));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Core.Contracts.Constants.ClaimNames.CustomUsername, user.Username),
                new Claim(Core.Contracts.Constants.ClaimNames.CustomUserId, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Type.ToString()),
                new Claim("TestPolicyPermission", "true")
            };

            var jwtIssuer =
#if DEBUG
                _configurationRoot["Jwt:Issuer"];
#else
                _configurationRoot["JWT_ISSUER"];
#endif
            var jwtAudience =
#if DEBUG
                _configurationRoot["Jwt:Audience"];
#else
                _configurationRoot["JWT_AUDIENCE"];
#endif
            var token = new JwtSecurityToken(jwtIssuer, jwtAudience,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Response<string> Login(UserLoginRequest request)
        {
            var response = new Response<string>();

            if (request.IsRequestInvalid(response))
                return response;

            var userResponse = First(x => x.Username.ToUpper() == request.Username.ToUpper());
            response.Merge(userResponse);
            if (response.NotOk)
                return response;

            return new Response<string>(GenerateJSONWebToken(userResponse.Payload));
        }

        public Response Register(UserRegisterRequest request)
        {
            var response = new Response();

            if (request.IsRequestInvalid(response))
                return response;

            var user = new UserEntity();
            user.InjectFrom(request);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.Type = UserType.User;
            base.Insert(user);

            return response;
        }

        public Response MarkLastSeen() =>
            new Response(Save(new UserSaveLastTimeSeenRequest(CurrentUser.Id)));

        public Response PromoteUser(UserPromoteRequest request) => 
            new Response(Save(request));
    }
}
