using FluentValidation;
using LSCore.Contracts.Exceptions;
using LSCore.Domain.Validators;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Domain.Validators.Proracuni;

public class ProracuniPutNUIDRequestValidator : LSCoreValidatorBase<ProracuniPutNUIDRequest>
{
    public ProracuniPutNUIDRequestValidator(OfficeDbContext dbContext)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Custom(
                (id, context) =>
                {
                    if (!dbContext.Proracuni.Any(x => x.Id == id))
                        throw new LSCoreNotFoundException();
                }
            );

        RuleFor(x => x.NUID).NotEmpty();
    }
}
