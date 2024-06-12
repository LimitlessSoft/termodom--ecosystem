using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using TD.Office.Common.Contracts.Entities;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using LSCore.Contracts;
using System.Text;
using TD.Office.Common.Contracts;

namespace TD.Office.Common.Domain.Extensions
{
    public static class UsrEntityExtensions
    {
        public static string GenerateJSONWebToken(this UserEntity user, IConfigurationRoot configurationRoot)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationRoot[Constants.Jwt.ConfigurationKey]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(LSCoreContractsConstants.ClaimNames.CustomUsername, user.Username),
                new Claim(LSCoreContractsConstants.ClaimNames.CustomUserId, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Type.ToString()),
            };

            var jwtIssuer = configurationRoot[Constants.Jwt.ConfigurationIssuer];
            var jwtAudience = configurationRoot[Constants.Jwt.ConfigurationAudience];
            var token = new JwtSecurityToken(jwtIssuer, jwtAudience,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
