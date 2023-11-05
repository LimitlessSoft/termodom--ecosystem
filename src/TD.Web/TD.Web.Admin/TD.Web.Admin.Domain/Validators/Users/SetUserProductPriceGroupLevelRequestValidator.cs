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
                .NotNull()
                .Must(z => dbContext.Users.Any(y => y.Id == z))
                    .WithMessage(UsersValidationCodes.UVC_018.GetDescription(String.Empty));

            RuleFor(x => x.ProductPriceGroupId)
                .NotNull()
                .Must(z => dbContext.ProductPriceGroups.Any(y => y.Id == z))
                    .WithMessage(UsersValidationCodes.UVC_019.GetDescription(String.Empty));

            RuleFor(x => x.Level)
                .NotNull()
                .Must(z => z >= 0 && z < Constants.NumberOfProductPriceGroupLevels)
                    .WithMessage(String.Format(UsersValidationCodes.UVC_020.GetDescription(String.Empty), Constants.NumberOfProductPriceGroupLevels));
        }
    }
}
