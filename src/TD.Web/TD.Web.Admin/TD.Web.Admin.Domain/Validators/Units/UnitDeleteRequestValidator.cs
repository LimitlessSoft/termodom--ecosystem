using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Units;
using Microsoft.EntityFrameworkCore;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Common.Repository;
using FluentValidation;

namespace TD.Web.Admin.Domain.Validators.Units;

public class UnitDeleteRequestValidator
    : LSCoreValidatorBase<UnitDeleteRequest>
{
    public UnitDeleteRequestValidator(WebDbContext dbContext)
    {
        RuleFor(x => x)
            .Must((request) =>
            {
                return !dbContext.Products
                    .Include(x => x.Unit)
                    .Any(x => x.Id == request.Id);
            })
            .WithMessage(UnitsValidationCodes.UVC_003.GetDescription());
    }
}