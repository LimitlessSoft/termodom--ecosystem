using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Validators.Users
{
    public class UserLoginRequestValidator : LSCoreValidatorBase<UserLoginRequest>
    {
        public UserLoginRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var user = dbContext.Users.FirstOrDefault(x => x.Username.ToUpper() == request.Username.ToUpper());

                    #region Checking legacy login. If legacy login, updating user password with new implementation so it can be validated

                    if (user != null && LegacyHashPassword(request.Password) == user.Password)
                    {
                        user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
                        dbContext.SaveChanges();
                    }
                    
                    string LegacySimpleHash(string value)
                    {
                        HashAlgorithm algorithm = SHA256.Create();
                        byte[] res = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in res)
                            sb.Append(b.ToString("X2"));

                        return sb.ToString();
                    }
                    string LegacyHashPassword(string RawPassword)
                    {
                        return LegacySimpleHash(LegacySimpleHash(LegacySimpleHash(LegacySimpleHash(LegacySimpleHash(LegacySimpleHash(RawPassword))))));
                    }
                    #endregion

                    if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
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
