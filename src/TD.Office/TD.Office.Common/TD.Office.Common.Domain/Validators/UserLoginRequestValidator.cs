using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Enums.ValidationCodes;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Common.Repository;

namespace TD.Web.Common.Domain.Validators.Users
{
    public class UserLoginRequestValidator : LSCoreValidatorBase<UsersLoginRequest>
    {
        private readonly IConfigurationRoot _configurationRoot;
        public UserLoginRequestValidator(OfficeDbContext dbContext, IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithErrorCode(UsersValidationCodes.UVC_001.GetDescription());

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode(UsersValidationCodes.UVC_001.GetDescription());

            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    // If there are no users in database, provided login user will be new master user
                    if(dbContext.Users.Count() == 0)
                    {
                        var masterUser = new UserEntity()
                        {
                            Username = request.Username!,
                            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password!),
                            Nickname = request.Username!,
                            Type = UserType.SuperAdministrator
                        };
                        dbContext.Users.Add(masterUser);
                        dbContext.SaveChanges();
                    }

                    var user = dbContext.Users
                        .Include(x => x.Permissions)
                        .Where(x =>
                            x.IsActive
                            && x.Username.ToUpper() == request.Username!.ToUpper()
                            && x.Permissions!.Count > 0
                            && x.Permissions!.Any(z =>
                                z.IsActive
                                && z.Permission == Permission.Access))
                        .AsNoTrackingWithIdentityResolution()
                        .FirstOrDefault();

                    if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
                    {
                        context.AddFailure(UsersValidationCodes.UVC_001.GetDescription());
                        return;
                    }
                });
        }
    }
}
