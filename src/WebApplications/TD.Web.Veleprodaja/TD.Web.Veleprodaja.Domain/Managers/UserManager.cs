using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TD.Core.Contracts.Extensions;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Veleprodaja.Contracts.DtoMappings;
using TD.Web.Veleprodaja.Contracts.Dtos.Users;
using TD.Web.Veleprodaja.Contracts.Entities;
using TD.Web.Veleprodaja.Contracts.IManagers;
using TD.Web.Veleprodaja.Contracts.Requests;
using TD.Web.Veleprodaja.Repository;

namespace TD.Web.Veleprodaja.Domain.Managers
{
    public class UserManager : BaseManager<UserManager, User>, IUserManager
    {
        private readonly IConfigurationRoot _configurationRoot;

        public UserManager(ILogger<UserManager> logger, VeleprodajaDbContext dbContext, IConfigurationRoot configurationRoot)
            : base(logger, dbContext)
        {
            _configurationRoot = configurationRoot;
        }

        #region Password Hash
        private static string SimpleHash(string value)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] res = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in res)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        private static string HashPassword(string RawPassword)
        {
            return SimpleHash(SimpleHash(SimpleHash(SimpleHash(SimpleHash(SimpleHash(RawPassword))))));
        }
        #endregion

        public Response<string> Authenticate(UsersAuthenticateRequest request)
        {
            var user = FirstOrDefault(x => x.Username == request.Username && x.Password == HashPassword(request.Password));
            if (user == null)
                return Response<string>.Unauthorized();

#if DEBUG
            var issuer = _configurationRoot["Jwt:Issuer"];
            var audience = _configurationRoot["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configurationRoot["Jwt:Key"]);
#else
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER").ToString();
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE").ToString();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY").ToString());
#endif

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                    new Claim(JwtRegisteredClaimNames.Email, request.Username),
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
            return new Response<string>(stringToken);
        }

        public Response<UsersMeDto> Me()
        {
            var response = new Response<UsersMeDto>();

            if (IsContextInvalid(response) || ContextUser == null || ContextUser.Identity == null)
                return response;

            string? name = ContextUser.GetName();
            if (string.IsNullOrWhiteSpace(name))
                return Response<UsersMeDto>.Unauthorized();
                
            var user = FirstOrDefault(x => x.Username == name);
            if (user == null)
                return Response<UsersMeDto>.Unauthorized();

            response.Payload = user.ToUsersMeDto();
            return response;
        }
    }
}
