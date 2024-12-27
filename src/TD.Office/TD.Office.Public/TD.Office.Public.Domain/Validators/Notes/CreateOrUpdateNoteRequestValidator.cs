using FluentValidation;
using LSCore.Contracts;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Domain.Validators.Notes;
public class CreateOrUpdateNoteRequestValidator
    : LSCoreValidatorBase<CreateOrUpdateNoteRequest>
{
    public CreateOrUpdateNoteRequestValidator(OfficeDbContext dbContext, LSCoreContextUser user) 
    {
        RuleFor(x => x.Name)
            .Custom((name, context) =>
            {
                if(dbContext.Notes.Any(x => x.Name.Equals(name) && x.IsActive && x.CreatedBy == user.Id))
                    context.AddFailure(NotesValidationCodes.NVC_002.GetDescription());
            });
    }
}
