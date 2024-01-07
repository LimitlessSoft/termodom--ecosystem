using LSCore.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Domain.Extensions
{
    public static class UserEntityExtensions
    {
        public static string GenerateJSONWebToken(this UserEntity user, IConfigurationRoot configurationRoot)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationRoot["JWT_KEY"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(LSCoreContractsConstants.ClaimNames.CustomUsername, user.Username),
                new Claim(LSCoreContractsConstants.ClaimNames.CustomUserId, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Type.ToString()),
                new Claim("TestPolicyPermission", "true")
            };

            var jwtIssuer = configurationRoot["JWT_ISSUER"];
            var jwtAudience = configurationRoot["JWT_AUDIENCE"];
            var token = new JwtSecurityToken(jwtIssuer, jwtAudience,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
