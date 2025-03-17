using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.KomercijalnoWebProductLinks
{
	public class KomercijalnoWebProductLinksSaveRequestValidator
		: LSCoreValidatorBase<KomercijalnoWebProductLinksSaveRequest>
	{
		public KomercijalnoWebProductLinksSaveRequestValidator(WebDbContext dbContext)
		{
			RuleFor(x => x.WebId).NotEmpty().GreaterThan(0);

			RuleFor(x => x.RobaId).NotEmpty().GreaterThan(0);

			RuleFor(x => x)
				.Custom(
					(request, context) =>
					{
						// If new link trying to be created but same robaId or webId already exists

						if (
							request.Id.HasValue
							&& !dbContext.KomercijalnoWebProductLinks.Any(x => x.Id == request.Id)
						)
							context.AddFailure(
								KomercijalnoWebProductLinksValidationCodes.KWPLVC_002.GetDescription()
							);

						if (
							(
								request.Id.HasValue
								&& dbContext.KomercijalnoWebProductLinks.Any(x =>
									x.Id != request.Id
									&& (x.RobaId == request.RobaId || x.WebId == request.WebId)
								)
							)
							|| (
								request.Id == null
								&& dbContext.KomercijalnoWebProductLinks.Any(x =>
									x.WebId == request.WebId || x.RobaId == request.RobaId
								)
							)
						)
							context.AddFailure(
								KomercijalnoWebProductLinksValidationCodes.KWPLVC_001.GetDescription()
							);
					}
				);
		}
	}
}
