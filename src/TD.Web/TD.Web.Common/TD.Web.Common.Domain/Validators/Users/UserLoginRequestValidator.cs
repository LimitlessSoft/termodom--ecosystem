using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Requests.Users;
using System.Security.Cryptography;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Common.Repository;
using FluentValidation;
using System.Text;
using Microsoft.EntityFrameworkCore;

// ReSharper disable InvertIf
// ReSharper disable RedundantJumpStatement

namespace TD.Web.Common.Domain.Validators.Users
{
    public class UserLoginRequestValidator : LSCoreValidatorBase<UserLoginRequest>
    {
        public UserLoginRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var user = dbContext.Users
                        .AsNoTrackingWithIdentityResolution()
                        .FirstOrDefault(x => x.Username.ToUpper() == request.Username.ToUpper());
                    
                    if (user == null)
                    {
                        context.AddFailure(UsersValidationCodes.UVC_006.GetDescription());
                        return;
                    }
                    
                    dbContext.Entry(user).Reload();

                    #region Checking legacy login. If legacy login, updating user password with new implementation so it can be validated

                    if (LegacyHashPassword(request.Password) == user.Password)
                    {
                        user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
                        dbContext.SaveChanges();
                    }
                    
                    string LegacySimpleHash(string value)
                    {
                        var res = SHA256.HashData(Encoding.UTF8.GetBytes(value));
                        var sb = new StringBuilder();
                        foreach (var b in res)
                            sb.Append(b.ToString("X2"));

                        return sb.ToString();
                    }
                    string LegacyHashPassword(string rawPassword)
                    {
                        return LegacySimpleHash(LegacySimpleHash(LegacySimpleHash(LegacySimpleHash(LegacySimpleHash(LegacySimpleHash(rawPassword))))));
                    }
                    #endregion
                    try
                    {
                        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
                        {
                            context.AddFailure(UsersValidationCodes.UVC_006.GetDescription());
                            return;
                        }
                    }
                    catch
                    {
                        context.AddFailure(UsersValidationCodes.UVC_006.GetDescription());
                        return;
                    }

                    if(user.ProcessingDate == null)
                    {
                        context.AddFailure(UsersValidationCodes.UVC_017.GetDescription());
                        return;
                    }
                });
        }
    }
}
