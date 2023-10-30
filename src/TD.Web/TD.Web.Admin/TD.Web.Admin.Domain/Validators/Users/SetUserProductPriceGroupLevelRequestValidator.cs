using FluentValidation;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Admin.Contracts;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Users
{
    public class SetUserProductPriceGroupLevelRequestValidator : ValidatorBase<SetUserProductPriceGroupLevelRequest>
    {
        public SetUserProductPriceGroupLevelRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .Must(z => dbContext.Users.Any(y => y.Id == z))
                    .WithMessage(UsersValidationCodes.UVC_018.GetDescription(String.Empty));

            RuleFor(x => x.ProductPriceGroupId)
                .Must(z => dbContext.ProductPriceGroups.Any(y => y.Id == z))
                    .WithMessage(UsersValidationCodes.UVC_019.GetDescription(String.Empty));

            RuleFor(x => x.Level)
                .Must(z => z >= 0 && z <= Constants.numberOfProductPriceGroupLevels)
                    .WithMessage(String.Format(UsersValidationCodes.UVC_020.GetDescription(String.Empty), Constants.numberOfProductPriceGroupLevels.ToString()));
        }
    }
}
