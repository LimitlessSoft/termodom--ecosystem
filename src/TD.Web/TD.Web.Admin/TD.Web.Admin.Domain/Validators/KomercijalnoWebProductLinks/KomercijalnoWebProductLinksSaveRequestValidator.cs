using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.KomercijalnoWebProductLinks
{
    public class KomercijalnoWebProductLinksSaveRequestValidator : LSCoreValidatorBase<KomercijalnoWebProductLinksSaveRequest>
    {
        public KomercijalnoWebProductLinksSaveRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x.WebId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.RobaId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    // If new link trying to be created but same robaId or webId already exists
                    var existingLink = dbContext.KomercijalnoWebProductLinks.FirstOrDefault(x => x.RobaId == request.RobaId || x.WebId == request.WebId);
                    if(request.IsOld && existingLink == null)
                        context.AddFailure(KomercijalnoWebProductLinksValidationCodes.KWPLVC_002.GetDescription());
                    if ((existingLink != null && request.IsNew) || (request.IsOld && existingLink != null && existingLink.Id != request.Id))
                        context.AddFailure(KomercijalnoWebProductLinksValidationCodes.KWPLVC_001.GetDescription());
                });
        }
    }
}
