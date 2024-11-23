using FluentValidation;
using LSCore.Domain.Validators;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Requests.Partneri;

namespace TD.Office.Public.Domain.Validators.Partners;
public class SaveKomercijalnoFinansijskoStatusRequestValidator : LSCoreValidatorBase<SaveKomercijalnoFinansijskoStatusRequest>
{
    public SaveKomercijalnoFinansijskoStatusRequestValidator(OfficeDbContext dbContext)
    {
        RuleFor(x => x.StatusId)
            .Custom((statusId, context) =>
            {
                var status = dbContext.KomercijalnoIFinansijskoPoGodinamaStatus.Where(x => x.IsActive && x.Id.Equals(statusId)).FirstOrDefault();
                if (status == null) {
                    context.AddFailure("Status ne postoji");
                }
            });
    }
}
